using Debug = UnityEngine.Debug;

public class DebugCustom
{
    public static void Log(params object[] content)
    {
#if  ROCKET_TEST
        string str="";
        for (int i = 0; i < content.Length; i++)
        {
            if(i== content.Length - 1)
            {
                str += content[i].ToString();
            }
            else
            {
                str += content[i].ToString() + "__";
            }
        }
        Debug.Log(str);
#endif
    }
    public static void LogError(params object[] content)
    {
#if  ROCKET_TEST
        string str = "";
        for (int i = 0; i < content.Length; i++)
        {
            if (i == content.Length - 1)
            {
                str += content[i].ToString();
            }
            else
            {
                str += content[i].ToString() + "__";
            }
        }
        Debug.LogError(str);
#endif
    }
    public static void LogColor(params object[] content)
    {
#if ROCKET_TEST
        string str = "";
        for (int i = 0; i < content.Length; i++)
        {
            if (i == content.Length - 1)
            {
                str +=content[i].ToString();
            }
            else
            {
                str += content[i].ToString() + "__";
            }
        }
        //Debug.Log(str);
        Debug.Log("<color=\"" + "#ffa500ff" + "\">" + str + "</color>");
#endif
    }
}
