using System.Collections.Generic;
using mvc104.models;
using StackExchange.Redis;
namespace mvc104{
    public  class highlevel{
      public static ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("47.93.226.74:8111");
    }

    public static class global{
      public static ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("47.93.226.74:8111");
      public static Dictionary<businessType,short> businesscount=new Dictionary<businessType,short>();
       static global(){
          businesscount.Add(businessType.ChangeLicense,5);
           businesscount.Add(businessType.delay,4);
            businesscount.Add(businessType.lost,3);
             businesscount.Add(businessType.damage,5);
              businesscount.Add(businessType.overage,5);

               businesscount.Add(businessType.expire,5);
                businesscount.Add(businessType.changeaddr,5);
                 businesscount.Add(businessType.basicinfo,4);
                  businesscount.Add(businessType.first,3);
                   businesscount.Add(businessType.network,4);

                    businesscount.Add(businessType.three,4);
                     businesscount.Add(businessType.five,4);
      }
    }
}