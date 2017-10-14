namespace mvc104.Controllers
{
    public partial class wechatController
    {
        public class wxpostreq {
            public string ToUserName;// 开发者微信号
            public string FromUserName;// 发送方帐号（一个OpenID）
            public string CreateTime;// 消息创建时间 （整型）
            public string MsgType;// text
            public string Content;//文本消息内容
            public string MsgId;//消息id，64位整型

            public string PicUrl;// 图片链接（由系统生成）
            public string MediaId;// 图片消息媒体id，可以调用多媒体文件下载接口拉取数据。
            public string Format;// 语音格式，如amr，speex等
            public string Recognition;// 语音识别结果，UTF8编码
            public string ThumbMediaId;// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据。

            public string Location_X;// 地理位置维度
            public string Location_Y;// 地理位置经度
            public string Scale;// 地图缩放大小
            public string Label;// 地理位置信息

            public string Title;// 消息标题
            public string Description;// 消息描述
            public string Url;// 消息链接
        }

    }
}
