namespace mvc104.models
{
    public enum responseStatus{ok,iderror,nameerror,phoneerror,tokenerror,requesterror,imageerror,fileprocesserror};
public class loginresponse{
public responseStatus status { get; set; }
public string token { get; set; }
}
public class commonresponse{
public responseStatus status { get; set; }
}
public class facerequest{
public string image { get; set; }
}
}