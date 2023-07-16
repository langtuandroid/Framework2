using Framework;

public static class MongoGameRegister
{
    public static void RegisterStruct()
    {
        MongoHelper.RegisterStruct<VTD_Id>();
        MongoHelper.RegisterStruct<VTD_EventId>();
    }
}