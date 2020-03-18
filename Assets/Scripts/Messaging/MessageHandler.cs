using System;
using System.Collections.Generic;

public static class MessageHandler
{
    private static Dictionary<Type, Action<object>> handles = new Dictionary<Type, Action<object>>();

    public static void RegisterReciever<T>(Action<T> toRegister)
    {
        var type = typeof(T);

        if (!handles.ContainsKey(type))
        {
            handles.Add(type, ConvertFromGeneric<T>(toRegister));
        }
        else
        {
            handles[type] += ConvertFromGeneric<T>(toRegister);
        }
    }

    public static void SendMessage<T>(T message)
    {
        var type = typeof(T);

        if (!handles.ContainsKey(type) || handles[type] == null)
        {
            return;
        }

        handles[type](message);
    }

    private static Action<object> ConvertFromGeneric<T>(Action<T> action)
    {
        return (object input) =>
        {
            action((T)input);
        };
    }
}
