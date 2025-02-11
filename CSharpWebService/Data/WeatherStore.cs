using CSharpWebService.Models;

namespace CSharpWebService.Data
{
    public static class WeatherStore
    {
        private static List<Weather> _weathers = new List<Weather>();
        
        static WeatherStore()
        {
            // 初始化一些测试数据
            _weathers.AddRange(new[]
            {
                new Weather { Id = 1, City = "北京", Temperature = 20, Condition = "晴朗", UpdateTime = DateTime.Now },
                new Weather { Id = 2, City = "上海", Temperature = 25, Condition = "多云", UpdateTime = DateTime.Now },
                new Weather { Id = 3, City = "广州", Temperature = 28, Condition = "阵雨", UpdateTime = DateTime.Now }
            });
        }

        public static IEnumerable<Weather> GetAll() => _weathers;
        public static Weather GetById(int id) => _weathers.FirstOrDefault(w => w.Id == id);
        
        private static int GenerateNewId()
        {
            return _weathers.Any() ? _weathers.Max(w => w.Id) + 1 : 1;
        }

        public static void Add(Weather weather)
        {
            weather.Id = GenerateNewId();
            _weathers.Add(weather);
        }

        public static void Update(Weather weather)
        {
            var index = _weathers.FindIndex(w => w.Id == weather.Id);
            if (index != -1)
                _weathers[index] = weather;
        }
        public static void Delete(int id) => _weathers.RemoveAll(w => w.Id == id);
    }
}
