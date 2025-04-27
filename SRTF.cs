using System.Collections.Generic;
using System.Linq;
using System;

class Process
{
    public int Process_id { get; set; }
    public int TurnaroundTime { get; set; }
    public int WaitingTime { get; set; }

    public int ArrivalTime { get; set; }
    public int BurstTime { get; set; }

    public int RemainingTime { get; set; }
    public int CompletionTime { get; set; }


    public Process(int process_id, int arrival_time, int burst_time)
    {
        Process_id = process_id;
        ArrivalTime = arrival_time;
        BurstTime = burst_time;
        RemainingTime = burst_time;
    }
}

class SRTF
{
    public static void schedule(List<Process> processList, int total_time)
    {
        int current_time = 0;
        int completed = 0;
        int total = processList.Count;

        int throughput = 0;
        List<Process> gantt_chart_process = new List<Process>();

        while (completed < total)
        {

            var pending = processList
                .Where(proc => proc.ArrivalTime <= current_time && proc.RemainingTime > 0)
                .ToList();

            if (pending.Count == 0)
            {
                current_time++;
                continue;
            }


            var current_process = pending
                .OrderBy(p => p.RemainingTime)
                .ThenBy(p => p.ArrivalTime)
                .First();


            current_process.RemainingTime--;
            gantt_chart_process.Add(current_process);

            if (current_process.RemainingTime == 0)
            {
                current_process.CompletionTime = current_time + 1;
                current_process.TurnaroundTime = current_process.CompletionTime - current_process.ArrivalTime;
                current_process.WaitingTime = current_process.TurnaroundTime - current_process.BurstTime;
                completed++;
            }

            current_time++;
        }

        displayGanttChart(gantt_chart_process);
        displayResult(processList);


        throughput = current_time / total;
        Console.WriteLine("Throughput Process:" + throughput);
        float cpu = ((float)total_time / (float)current_time) * 100;
        Console.WriteLine("CPU Utilization  :" + cpu + "%");
    }

    private static void displayResult(List<Process> processList)
    {
        Console.WriteLine("\nProcess Information:");
        Console.WriteLine("PID\tArrivalTime\tBurstTime\tCompletionTime\tTurnaround\tWaitingTime");
        int avg_turn_time = 0;
        int avg_wait_time = 0;
        foreach (var proc in processList)
        {
            avg_turn_time += proc.TurnaroundTime;
            avg_wait_time += proc.WaitingTime;
            Console.WriteLine($"{proc.Process_id}\t{proc.ArrivalTime}\t\t{proc.BurstTime}\t\t{proc.CompletionTime}\t\t{proc.TurnaroundTime}\t\t{proc.WaitingTime}");
        }

        Console.WriteLine("Average Waiting Time:" + avg_wait_time / processList.Count);
        Console.WriteLine("Average Turnaround Time:" + avg_turn_time / processList.Count);

    }

    private static void displayGanttChart(List<Process> processList)
    {
        Console.WriteLine("\nGantt Chart:");
        foreach (var proc in processList)
        {
            Console.Write($"P{proc.Process_id} ");
        }
        Console.WriteLine();

        for (int id = 0; id <= processList.Count; id++)
        {
            if (id < 10)
                Console.Write(id == 0 ? "0  " : $"{id}  ");
            else
                Console.Write(id == 0 ? "0  " : $"{id} ");

        }
        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        int totalProcess = 0;
        int all_process_time = 0;
        char c = 'A';
        List<Process> processes = new List<Process>();

        Console.Write("Welcome To  SRTF Scheduling\n");
        Console.Write("Please enter number of process: ");
        var input = Console.ReadLine();
        if (input != null)
            totalProcess = int.Parse(input);


        for (int i = 0; i < totalProcess; i++, c++)
        {

            Console.Write($"\nPlease enter arrival time and burst time of process {c}: ");
            var @params = Console.ReadLine().Split();
            int ArrivalTime = int.Parse(@params[0]);
            int BurstTime = int.Parse(@params[1]);
            Process process = new Process(i + 1, ArrivalTime, BurstTime);
            processes.Add(process);
            all_process_time += BurstTime;
        }
        SRTF.schedule(processes, all_process_time);
    }
}