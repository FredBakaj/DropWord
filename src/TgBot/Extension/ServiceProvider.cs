namespace DropWord.TgBot.Extension
{
    public static class ServiceProvider
    {
        /// <summary>
        /// Метод розширення, дозволяє взяти реалізацію інтерфейсу.
        /// Допомагає у випадках, коли є багато реалізацій які
        /// відносяться до одного інтерфейсу
        /// </summary>
        /// <typeparam name="IT">Тип інтерфейса</typeparam>
        /// <typeparam name="T">Тип реалізації</typeparam>
        /// <returns>Вертає обєкт реалізації</returns>
        public static T GetSpecificService<IT, T>(this IServiceProvider services) where T : IT 
        {
            IEnumerable<IT> allService = services.GetServices<IT>();
            foreach (IT service in allService)
            {
                if (service is not null)
                {
                    if (service.GetType() == typeof(T))
                    {
                        return (T)service;
                    } 
                }
                
            }
            throw new NullReferenceException($"Di not have a specific servise {typeof(T)} there implementation {typeof(IT)}");
        }
    }
}
