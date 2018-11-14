namespace IcsokaPayments.Domain.Entities
{
    public class Setting:BaseEntity
    {
        public Setting() { }

		public Setting(string name, string value)
		{
            Name = name;
            Value = value;
			
        }
        
       
		public string Name { get; set; }

        
		public string Value { get; set; }

		
    
    }
}