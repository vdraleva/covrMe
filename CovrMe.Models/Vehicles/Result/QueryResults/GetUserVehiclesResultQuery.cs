using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Vehicles.Result.QueryResults
{
    public class GetUserVehiclesResultQuery
    {
        public GetUserVehiclesResultQuery()
        {
            this.Vehicles = new GetUserVehiclesResultModel();
        }
        public GetUserVehiclesResultModel Vehicles { get; set; }
    }
}
