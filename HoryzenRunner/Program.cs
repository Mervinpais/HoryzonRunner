namespace HoryzenRunner
{
    public class Program
    {
        public static string VHD = "";
        public static void Main(string[] args)
        {
            /*BasicInstructionSet basicInstructionSet = new BasicInstructionSet();
            Memory_4bit memory_4Bit = new Memory_4bit();
            PartitionTable partitionTable = new PartitionTable();
            OS_Setup.SetupOS(args, out memory_4Bit, out partitionTable);
            basicInstructionSet.Run_4Bit(memory_4Bit);
            */

            //We assume the GUI has launched this app directly

            if (args.Length == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    """
                    Unhandled exception. System.UnauthorizedAccessException: Attempted to perform an unauthorized operation.
                        at HoryzenRunner.Program.Main | See below

                        DO NOT RUN FAKOS/HORYZENRUNNER DIRECTLY WITHOUT GUI OR ARGS
                        ABORT! ABORT! ABORT!
                    """
                );
                Console.ResetColor();
                Environment.Exit(-1);
            }

            if (!File.Exists(args[0]))
            {
                throw new FileNotFoundException($"ERROR: VHD Not found, FILENAME: {args[0]}");
            }

            VHD = args[0];
            SysFS.Initialize();
            Kernel.Initialize();
        }
    }
}
