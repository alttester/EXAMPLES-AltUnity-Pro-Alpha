using System;
using Altom.AltUnityDriver;
using Altom.AltUnityDriver.Commands;
using Assets.AltUnityTester.AltUnityServer.Communication;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityUnloadSceneCommand : AltUnityCommand<AltUnityUnloadSceneParams, string>
    {
        readonly ICommandHandler handler;

        public AltUnityUnloadSceneCommand(ICommandHandler handler, AltUnityUnloadSceneParams cmdParams) : base(cmdParams)
        {
            this.handler = handler;
        }

        public override string Execute()
        {
            string response = AltUnityErrors.errorNotFoundMessage;
            try
            {
                var sceneLoadingOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(CommandParams.sceneName);
                if (sceneLoadingOperation == null)
                {
                    throw new CouldNotPerformOperationException("Cannot unload scene: " + CommandParams.sceneName);
                }
                sceneLoadingOperation.completed += sceneUnloaded;
            }
            catch (ArgumentException)
            {
                throw new CouldNotPerformOperationException("Cannot unload scene: " + CommandParams.sceneName);
            }

            response = "Ok";
            return response;
        }

        private void sceneUnloaded(UnityEngine.AsyncOperation obj)
        {
            handler.Send(ExecuteAndSerialize(() => "Scene Unloaded"));
        }
    }
}