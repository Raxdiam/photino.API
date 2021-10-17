namespace PhotinoAPI.Modules
{
    public abstract class PhotinoModuleBase : IPhotinoModule
    {
        protected PhotinoModuleBase() {}
        protected PhotinoModuleBase(PhotinoApi api)
        {
            Api = api;
        }

        protected PhotinoApi Api { get; }
    }
}
