using Li.Lan.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Security.Cryptography;

namespace Li.Lan.Common.Services
{
    public interface ICommonService
    {
        ApplicationContext GetApplicationContext();

        DateTime GetCurrentDateTime();

        DateTime GetCurrentDateTimeUtc();

        Guid CreateGuid();

        long GenerateRandomInt64();

        int GenerateRandomInt32();

        Random CreateCryptoRandom();

        string SerializeToJson(object obj);

        T DeserializeFromJson<T>(string json);

        T DeepCopy<T>(T obj);
    }

    public class CommonService : ICommonService
    {
        public CommonService(IApplicationContextProvider applicationContextProvider)
        {
            this.ApplicationContextProvider = applicationContextProvider;

            this.DateTimeTypeJsonConverter = new IsoDateTimeConverter();
        }

        private IApplicationContextProvider ApplicationContextProvider { get; set; }

        private JsonConverter DateTimeTypeJsonConverter { get; set; }

        public ApplicationContext GetApplicationContext()
        {
            return this.ApplicationContextProvider.GetApplicationContext();
        }

        public DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        public DateTime GetCurrentDateTimeUtc()
        {
            return DateTime.UtcNow;
        }

        public Guid CreateGuid()
        {
            return Guid.NewGuid();
        }

        public long GenerateRandomInt64()
        {
            var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[sizeof(long)];
            rng.GetBytes(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

        public int GenerateRandomInt32()
        {
            var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[sizeof(int)];
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public Random CreateCryptoRandom()
        {
            return new Random(GenerateRandomInt32());
        }

        public string SerializeToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, this.DateTimeTypeJsonConverter);
        }

        public T DeserializeFromJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, this.DateTimeTypeJsonConverter);
        }

        public T DeepCopy<T>(T obj)
        {
            var json = this.SerializeToJson(obj);
            T dc = this.DeserializeFromJson<T>(json);
            return dc;
        }
    }
}