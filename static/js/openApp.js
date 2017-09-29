// JavaScript Document

window._web_public_app = {
	getUa: function () {
		var ua = window.navigator.userAgent.toLocaleLowerCase(), isApple = !!ua.match(/(ipad|iphone|mac)/i), isAndroid = !!ua.match(/android/i), isWinPhone = !!ua.match(/MSIE/i), ios6 = !!ua.match(/os 6.1/i);
		return { isApple: isApple, isAndroid: isAndroid, isWinPhone: isWinPhone, ios6: ios6 }
	},
	initOpenApp: function (url) {
		var _uaName = this.browserName(), platforms = this.getUa();
		if(_uaName != "WeiXin"){
			if (platforms.isAndroid) {
				if (_uaName != "sogousearch") {
                    window.location.href = url;
                }
                else {
                    this.iframeWake(url);
                }
			}else if(platforms.isApple){
				if (_uaName == "Chrome" || _uaName == "Crios" || _uaName == "Safari" || _uaName == "baidubrowser") {
                    window.location.href = url;//兼容谷歌浏览器
                }
                else {
					this.iframeWake(url);
				}
			}
		}
	},
	openApp: function (url) {
		var _uaName = this.browserName(), platforms = this.getUa();
		var downUrl = 'https://www.zongyoung.com/index.php?s=/Home/Download/driverdown.html';
		
		if(_uaName == "WeiXin"){
			this.loadHtml();
			this.loadStyleText();
		}else if (platforms.isAndroid) {
			if (_uaName == "sogousearch") {
                window.location.href = url;
			}
			else {
				this.iframeWake(url);
			}
			setTimeout(function () {
				if (_uaName != "sogousearch") {
					window.location.href = downUrl;//兼容搜狗浏览器
				}else {
					_web_public_app.iframeWake(downUrl);
				}
			}, 1000);
		}else if (platforms.isApple) {
			if (_uaName == "Chrome" || _uaName == "Crios" || _uaName == "Safari" || _uaName == "baidubrowser") {
				window.location.href = url;//兼容谷歌浏览器
			}
			else {
				this.iframeWake(url);
			}
			setTimeout(function () {
				if (_uaName == "Chrome" || _uaName == "Crios" || _uaName == "Safari" || _uaName == "baidubrowser") {
					window.location.href = downUrl;
				}else {
					_web_public_app.iframeWake(downUrl);
				}
			}, 2000);
			
		}
	},
	iframeWake: function (url) {
		var c = document.createElement("div");
		c.style.visibility = "hidden";
		c.innerHTML = '<iframe src="' + url + '" scrolling="no" width="1" height="1"></iframe>';
		document.body.appendChild(c);
	},
	/*
	 * 获取浏览器名称
	 * */
	browserName: function () {
		var ua = window.navigator.userAgent.toLowerCase();
		var browser = "";
		if (window.ActiveXObject) {
			browser = "IE"
		} else {
			if (document.getBoxObjectFor || ua.indexOf("firefox") > -1) {
				browser = "Firefox"
			} else {
				if(ua.indexOf("weibo") > -1){
					browser = "WeiBo";
				}else{
					if(ua.indexOf("alipay") > -1){
						browser = "Alipay";
					}else{
						if (ua.indexOf("micromessenger") > -1) {
							browser = "WeiXin";
						}
						else {
							if (ua.indexOf("baidu") != -1) {
								browser = "baidubrowser";
							} else {
								if (ua.indexOf("ucbrowser") != -1) {
									browser = "ucbrowser";
								} else {
									if (ua.indexOf("miuibrowser") > -1) {
										browser = "miuibrowser";
									} else {
										if (ua.indexOf("lbbrowser") > -1) {
											browser = "lbbrowser";
										}
										else {
											if (ua.indexOf("qqbrowser") > -1) {
												browser = "qqbrowser";
											}
											else {
												if (ua.indexOf("qhbrowser") > -1) {
													browser = "qhbrowser";
												} else {
													if (ua.indexOf("hao123") > -1) {
														browser = "hao123";
													} else {
														if (ua.indexOf("sogoumobilebrowser") > -1) {
															browser = "sogousearch";
														} else {
															if (ua.indexOf("crios") > -1) {
																browser = "Crios";
															} else {
																if (ua.indexOf("chrome") > -1) {
																	browser = "Chrome";
																} else {
																	if (window.opera) {
																		browser = "Opera";
																	} else {
																		if (ua.indexOf("safari") > -1) {
																			browser = "Safari";
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		return browser;
	},
	loadHtml:function (){
		var div = document.createElement('div');
		div.id = 'weixin-tip';
		div.innerHTML = '<p><img src="/Home/images/live_weixin.png" alt="微信打开"/></p>';
		document.body.appendChild(div);
    },
    loadStyleText:function() {
		var cssText = "#weixin-tip{position: fixed; left:0; top:0; background: rgba(0,0,0,0.8); filter:alpha(opacity=80); width: 100%; height:100%; z-index: 100;} #weixin-tip p{text-align: center; margin-top: 10%; padding:0 5%;}";
		var style = document.createElement('style');
		style.rel = 'stylesheet';
		style.type = 'text/css';
		try {
			style.appendChild(document.createTextNode(cssText));
		} catch (e) {
			style.styleSheet.cssText = cssText; //ie9以下
		}
		var head=document.getElementsByTagName("head")[0]; //head标签之间加上style样式
		head.appendChild(style); 
	}
}


var app = {
	// 成功提示信息
	success	: function(info){
		app_msg('success',info);
	},
	// 错误提示信息
	error : function(info){
		app_msg('error',info);
	},
	// 普通提示信息
	warning : function(info){
		app_msg('warning',info);
	},
	// 普通提示信息
	danger : function(info){
		app_msg('danger',info);
	}
}

function app_msg(status,info){
	if(_web_public_app.getUa().isApple){
		window.webkit.messageHandlers.JSALertView.postMessage([status,info]);
	}else if(_web_public_app.getUa().isAndroid){ 
		window.jsObj.JSALertView(info);
	}
}
