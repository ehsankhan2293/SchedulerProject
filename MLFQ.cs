using System;

class Process
{
    public int Id;
    public int ArrivalTime;
    public int BurstTime;
    public int WaitingTime;
    public int TurnAroundTime;
    public int ResponseTime;
    public int CompletionTime;
}

class MLFQ
{
    static int MAX_PROCESS = 1000;
    //Three queues are used, each hold upto 1000 process 
    static Process[] Queue1 = new Process[MAX_PROCESS];
    static Process[] Queue2 = new Process[MAX_PROCESS];
    static Process[] Queue3 = new Process[MAX_PROCESS]; 
    static int totalProcess;

    // This function is used to sort the process based on its arrival time
    static void SortProcess()
    {
        Process temp;
        for (int i = 0; i < totalProcess; i++)
        {
            for (int j = i + 1; j < totalProcess; j++)
            {
                if (Queue1[i].ArrivalTime > Queue1[j].ArrivalTime)
                {
                    temp = Queue1[i];
                    Queue1[i] = Queue1[j];
                    Queue1[j] = temp;
                }
            }
        }
    }

    //DRIVER CODE
    static void Main()
    {
        int k = 0;
        int round = 0;
        int current_time = 0;
        int time_quantum_1 = 5;
        int time_quantum_2 = 8;
        int all_process_time = 0;
        int flag = 0;
        int avg_waiting_time = 0;
        int avg_turn_time = 0;
        int c=1;
        List<Process> gantt_chart_process = new List<Process>();


        Console.Write("Welcome To Multi Level Feedback Queue Scheduling\n");
        Console.Write("Please enter number of process: ");
        var input = Console.ReadLine();
        if (input != null)
            totalProcess = int.Parse(input);

        for (int i = 0; i < totalProcess; i++)
        {
            Queue1[i] = new Process();
            Queue2[i] = new Process();
            Queue3[i] = new Process();
        }

        for (int i = 0; i < totalProcess; i++, c++)
        {
            Queue1[i].Id = c;
            Console.Write($"\nPlease enter arrival time and burst time of process {Queue1[i].Id}: ");
            var @params = Console.ReadLine().Split();
            Queue1[i].ArrivalTime = int.Parse(@params[0]);
            Queue1[i].BurstTime = int.Parse(@params[1]);
            Queue1[i].ResponseTime = Queue1[i].BurstTime;
            all_process_time += Queue1[i].BurstTime;
        }

        SortProcess();
        Console.WriteLine("========================================================");
        current_time = Queue1[0].ArrivalTime;
        Console.WriteLine("Process status in first queue with time quantum=5");
        Console.WriteLine("Process\t\tRT\t\tWT\t\tTAT\t\t");
        int found = 0;
        for (int i = 0; i < totalProcess; i++)
        {
            
            if (Queue1[i].ResponseTime <= time_quantum_1)
            {
                gantt_chart_process.Add(Queue1[i]);
                current_time += Queue1[i].ResponseTime; 
                Queue1[i].ResponseTime = 0;
                Queue1[i].WaitingTime = current_time - Queue1[i].ArrivalTime - Queue1[i].BurstTime; 
                Queue1[i].TurnAroundTime = current_time - Queue1[i].ArrivalTime; 
                Console.WriteLine($"{Queue1[i].Id}\t\t{Queue1[i].BurstTime}\t\t{Queue1[i].WaitingTime}\t\t{Queue1[i].TurnAroundTime}");
                avg_waiting_time += Queue1[i].WaitingTime;
                avg_turn_time += Queue1[i].TurnAroundTime;
                found = 1;
            }
            else 
            {
                gantt_chart_process.Add(Queue2[k]);
                Queue2[k].WaitingTime = current_time;
                current_time += time_quantum_1;
                Queue1[i].ResponseTime -= time_quantum_1;
                Queue2[k].BurstTime = Queue1[i].ResponseTime;
                Queue2[k].ResponseTime = Queue2[k].BurstTime;
                Queue2[k].Id = Queue1[i].Id;
                k++;
                flag = 1;
            }
        }

        if (found == 0)
        {
            Console.WriteLine("\n No process found\n");
        }
        Console.WriteLine("========================================================");
        if (flag == 1)
        {
            Console.WriteLine("Process status in second queue with time quantum=8");
            Console.WriteLine("Process\t\tRT\t\tWT\t\tTAT\t\t");
        }

        for (int i = 0; i < k; i++)
        {
            if (Queue2[i].ResponseTime <= time_quantum_2)
            {
                gantt_chart_process.Add(Queue2[i]);
                current_time += Queue2[i].ResponseTime; 
                Queue2[i].ResponseTime = 0;
                Queue2[i].WaitingTime = current_time - time_quantum_1 - Queue2[i].BurstTime; 
                Queue2[i].TurnAroundTime = current_time - Queue2[i].ArrivalTime; 
                Console.WriteLine($"{Queue2[i].Id}\t\t{Queue2[i].BurstTime}\t\t{Queue2[i].WaitingTime}\t\t{Queue2[i].TurnAroundTime}");
                avg_waiting_time += Queue2[i].WaitingTime;
                avg_turn_time += Queue2[i].TurnAroundTime;
            }
            else 
            {
                gantt_chart_process.Add(Queue3[round]);
                Queue3[round].ArrivalTime = current_time;
                current_time += time_quantum_2;
                Queue2[i].ResponseTime -= time_quantum_2;
                Queue3[round].BurstTime = Queue2[i].ResponseTime;
                Queue3[round].ResponseTime = Queue3[round].BurstTime;
                Queue3[round].Id = Queue2[i].Id;
                round++;
                flag = 2;
            }
        }
        Console.WriteLine("========================================================");
        if (flag == 2)
            Console.WriteLine("\nProcess status third queue using FCFS ");

        for (int i = 0; i < round; i++)
        {
            if (i == 0)
                Queue3[i].CompletionTime = Queue3[i].BurstTime + current_time - time_quantum_1 - time_quantum_2;
            else
                Queue3[i].CompletionTime = Queue3[i - 1].CompletionTime + Queue3[i].BurstTime;
        }

        for (int i = 0; i < round; i++)
        {
            Queue3[i].TurnAroundTime = Queue3[i].CompletionTime;
            Queue3[i].WaitingTime = Queue3[i].TurnAroundTime - Queue3[i].BurstTime;
            Console.WriteLine($"{Queue3[i].Id}\t\t{Queue3[i].BurstTime}\t\t{Queue3[i].WaitingTime}\t\t{Queue3[i].TurnAroundTime}\t\t");

            avg_waiting_time += Queue3[i].WaitingTime;
            avg_turn_time += Queue3[i].TurnAroundTime;
        }
    //      Console.WriteLine("========================================================");
    //displayGanttChart(gantt_chart_process);
        Console.WriteLine("========================================================");
        Console.WriteLine("CUrrent time " + current_time
            );
        float cpu = ((float)all_process_time / (float)current_time) * 100;
        Console.WriteLine("Average Waiting Time :" + avg_waiting_time / totalProcess);
        Console.WriteLine("Average Turn Around Time :" + avg_turn_time / totalProcess);
        Console.WriteLine("Throughput process :" + current_time / totalProcess);
        Console.WriteLine("CPU Utilization  :" + cpu + "%");

    }

    private static void displayGanttChart(List<Process> processList)
    {
        Console.WriteLine("\nGantt Chart:");
        foreach (var proc in processList)
        {
            Console.Write($"P{proc.Id} ");
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