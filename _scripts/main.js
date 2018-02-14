var nav = false;
$("button").click(function() {
    $("button").removeClass('active');
    if($(event.target).attr('id')=='HomeButton'){
        $("section").load("home.aspx");
        $("#HomeButton").addClass('active');
    } else if($(event.target).attr('id')=='StatsButton'){
        $("section").load("stats.aspx");
        $("#StatsButton").addClass('active');
    } else if($(event.target).attr('id')=='SupportButton'){
        $("section").load("support.aspx");
        $("#SupportButton").addClass('active');
    } else if($(event.target).attr('id')=='ConfigButton'){
        $("section").load("config.aspx");
        $("#ConfigButton").addClass('active');
    } else if($(event.target).attr('id')=='AddUser'){
        $("section").load("register.aspx");
        $("#AddUser").addClass('active');
    } else if($(event.target).attr('id')=='EditUser'){
        $("section").load("edit.aspx");
        $("#EditUser").addClass('active');
    } else if($(event.target).attr('id')=='DeleteUser'){
        $("section").load("delete.aspx");
        $("#DeleteUser").addClass('active');
    } else if($(event.target).attr('id')=='NavButton'){
        if(nav){
            $(".side-bar").css("width", "0vw");
            nav = false;
        } else {
            $(".side-bar").css("width", "25vw");
            nav = true;
        }
    }
});