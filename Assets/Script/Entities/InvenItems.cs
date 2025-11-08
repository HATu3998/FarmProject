using Newtonsoft.Json;
using UnityEngine;
namespace MyProject.Data
{
    public class InvenItems 
    {
        public string name { get; set; }
        public string descrition { get; set; }

        public InvenItems()
        {

        }
        public InvenItems(string name, string descritpion)
        {
            this.name = name;
            this.descrition = descritpion;
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}