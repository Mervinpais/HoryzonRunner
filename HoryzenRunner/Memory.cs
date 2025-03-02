using System;
using System.Security.Permissions;

namespace HoryzenRunner
{
    public class MemoryAddress {
        private int address;

        private object? vaLue = null;

        public int address_ {
            get { return address; }
            set { address = value; }
        }

        public string value_ {
            get { return vaLue.ToString(); }
            set { vaLue = value; }
        }
    }

    public class Variable : MemoryAddress {
        private string name;
        private string type = "object";
        public string name_ {
            get { return name; }
            set { name = value; }
        }
        public string type_ {
            get { return type; }
            set { type = value; }
        }

    }

    public class Memory
    {
        bool initialized = false;
        
        public List<MemoryAddress> RAM = new List<MemoryAddress>();
        int ramLimit = 64; //code for that (64kb)

        void Init() {
            for (int i = 0; i < ramLimit; i++) {
                RAM.Add(new MemoryAddress());
            }
            initialized = true;
        }

        public void RunChecks()
        {
            if (!initialized) Init();
        }
        public void AddToMemory(int address, object data) {
            RunChecks();
            RAM[address].value_ = data.ToString();
        }

    }
}