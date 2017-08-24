using StackExchange.Redis;
namespace mvc104{
    public  class highlevel{
      public static ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("47.93.226.74:8111");
 
    }
}