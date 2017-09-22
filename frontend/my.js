//var aa=jQuery.noConflict();
jQuery(function($){
    $("body").on("click","a",function(event){
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
  bb.clone(true).insertBefore("div");
  // $("div").on("click","a",{aa:"bb"},function(event){
  //     console.log("event object:"+event.data.aa);
  //     console.dir(event)
  // });

// Switching handlers using the `.one()` method
    $( "div" ).one( "click", function() {
      //  $(this).text("----");
        console.log( "You just clicked this for the first time!" );
        $( this ).click(function() {
            $(this).text("92349230492034");
            console.log( "You have clicked this before!" );
        });
    });
//  $("a").on(a,b,c,d);
});