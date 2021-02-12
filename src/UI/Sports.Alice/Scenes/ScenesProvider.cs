using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Sports.Alice.Scenes
{
    public class ScenesProvider : IScenesProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ScenesProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Scene GetScene(SceneType sceneType)
        {
            return sceneType switch
            {
                SceneType.Welcome => GetScene<WelcomeScene>(),
                SceneType.LatestNews => GetScene<LatestNewsScene>(),
                SceneType.MainNews => GetScene<MainNewsScene>(),
                SceneType.BestComments => GetScene<BestCommentsScene>(),
                SceneType.Help => GetScene<HelpScene>(),
                _ => throw new Exception("Unknown scene"),
            };
        }

        private T GetScene<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
