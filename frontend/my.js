var aa=jQuery.noConflict();
aa(function(){
    aa("a").click(function(event){
        alert("你-dfad--好1");
        event.preventDefault();
       // $(this).hide("slow");
    });
    aa("a").addClass("test");
});