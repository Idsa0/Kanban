using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class DataService
    {
        // private DataFacade df;

        public DataService() { }

        // internal DataFacade Df { set { df = value; } }

        ///<summary>This method loads all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method.
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        /// <exception cref="IOException">Thrown if there was a problem when accessing the data</exception>
        public string LoadData()
        {
            Response res;
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b>
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        ///<returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        /// <exception cref="IOException">Thrown if there was a problem when accessing the data</exception>
        public string DeleteData()
        {
            Response res;
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }
    }
}
