//var aa=jQuery.noConflict();
jQuery(function($){
    $("a").click(function(event){
        alert("你---好1");
        event.preventDefault();
       // $(this).hide("slow");
    });
    $("a").addClass("test");
   var  bb= $("<a/>",{
        html:"hahah",
        "class":"nee",
        href:"http://www.sina.com"
    }).appendTo("#mydiv");
  bb.clone(true).insertBefore("body");
});