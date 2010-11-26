namespace Limaki.Model.Streams {
    public class Magic {
        public Magic(){}
        public Magic(byte[] magic, int offset){
            this.Offset = offset;
            this.Bytes = magic;
            this.OffsetIsRange = false;
        }
        public int Offset{get;set;}

        /// <summary>
        /// somewhere between 0 and offset bytes are to be found
        /// </summary>
        public bool OffsetIsRange { get; set; }
        public byte[] Bytes {get;set;}
    }
}