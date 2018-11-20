using System;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

/// <summary>
/// ExtHelper 的摘要说明
/// </summary>
public sealed class ExtHelper
{
    #region MemberVariable，记录ExtJS基本文件的路径
    /// <summary>
    /// ExtJS存放的路径，如果在WebConfig中有配置，那么就根据配置里面的，否则就用当前目录下面的。这样有利于使用
    /// 不同版本的ExtJS库，也有利于系统的升级。
    /// </summary>
    public static readonly string EXT_BASE = ConfigurationManager.AppSettings["EXT_BASE"] ?? "Ext";
    /// <summary>
    /// ext-all.css
    /// </summary>
    public static readonly string EXT_CSS_ALL = EXT_BASE + "/resources/css/ext-all.css";
    /// <summary>
    /// xtheme-green.css
    /// </summary>
    public static readonly string EXT_CSS_DEFAULTTHEME = EXT_BASE + "/resources/css/xtheme-green.css";
    /// <summary>
    /// ext-all.js
    /// </summary>
    public static readonly string EXT_JS_ALL = EXT_BASE + "/ext-all.js";
    /// <summary>
    /// ext-base.js
    /// </summary>
    public static readonly string EXT_JS_BASE = EXT_BASE + "/adapter/ext/ext-base.js";
    /// <summary>
    /// ext-lang-zh_CN.js
    /// </summary>
    public static readonly string EXT_JS_LANGUAGE = EXT_BASE + "/ext-lang-zh_CN.js";
    /// <summary>
    /// 缺省空白图片地址
    /// </summary>
    //public static readonly string EXT_BLANK_IMAGE_URL = EXT_BASE + "/resource/images/default/s.gif";

    /// <summary>
    ///  0    ext-all.css
    ///  1    ext-base.js
    ///  2    ext-all.js
    ///  3    ext-lang-zh_CN.js
    /// </summary>
    private static readonly IList<HtmlGenericControl> extresource;

  #endregion

      #region Constructors

      static ExtHelper()
      {
          extresource = new List<HtmlGenericControl>();

        //ext-all.css
        HtmlGenericControl css_ext_all = new HtmlGenericControl("link");
        css_ext_all.Attributes.Add("type", "text/css");
        css_ext_all.Attributes.Add("rel", "stylesheet");
        css_ext_all.Attributes.Add("href", EXT_CSS_ALL);
        extresource.Add(css_ext_all);


        //ext-base.js
        HtmlGenericControl js_ext_base = new HtmlGenericControl("script");
        js_ext_base.Attributes.Add("type", "text/javascript");
        js_ext_base.Attributes.Add("src", EXT_JS_BASE);
        extresource.Add(js_ext_base);

        //ext-all.js
        HtmlGenericControl js_ext_all = new HtmlGenericControl("script");
        js_ext_all.Attributes.Add("type", "text/javascript");
        js_ext_all.Attributes.Add("src", EXT_JS_ALL);
        extresource.Add(js_ext_all);

        //ext-lang-zh_CN.js
        HtmlGenericControl js_ext_lang = new HtmlGenericControl("script");
        js_ext_lang.Attributes.Add("type", "text/javascript");
        js_ext_lang.Attributes.Add("src", EXT_JS_LANGUAGE);
        extresource.Add(js_ext_lang);

        //xtheme-green.css
        HtmlGenericControl css_ext_default = new HtmlGenericControl("link");
        css_ext_default.Attributes.Add("type", "text/css");
        css_ext_default.Attributes.Add("rel", "stylesheet");
        css_ext_default.Attributes.Add("href", EXT_CSS_DEFAULTTHEME);
        extresource.Add(css_ext_default);

        //BLANK_IMAGE_URL
        //HtmlGenericControl js_ext_image = new HtmlGenericControl("script");
        //js_ext_image.Attributes.Add("type", "text/javascript");
        //js_ext_image.InnerHtml = EXT_BLANK_IMAGE_URL;
        //extresource.Add(js_ext_image);


      }

      #endregion

      #region Method

       /// <summary>
       /// 添加资源文档
       /// </summary>
       /// <param name="head"></param>
       /// <param name="page"></param>
      public static void Add(HtmlHead head, System.Web.UI.Page page)
      {
          if (head != null)
          {
              if (extresource != null)
              {
                  //head.Controls[0]为title

                  head.Controls.AddAt(1, extresource[0]);
                  head.Controls.AddAt(2, extresource[1]);
                  head.Controls.AddAt(3, extresource[2]);
                  //head.Controls.AddAt(4, extresource[3]);
                  head.Controls.AddAt(4, extresource[4]);
              }
          }
      }

      #endregion

}
