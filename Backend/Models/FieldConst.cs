using System;

namespace SIMP.Models{
    
    public class FieldConst{
        
        public FieldConst(string Sequence, String name){
            this.SEQUENCE = Sequence;
            this.NAME = name;
        }

        public string SEQUENCE { get; }

        public string NAME { get; }

        override
        public string ToString(){ 
            return this.NAME;
        }

    }
}
