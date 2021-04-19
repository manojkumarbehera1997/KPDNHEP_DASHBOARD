$(document).ready(function () {
    var $tabs = $("#tabs").tabs(),
        $dialog = $("#new_tab_dialog").dialog({
            autoOpen: false,
            height: 200,
            width: 350,
            modal: true,
            buttons: {
                "Create a new tab": function () {
                    $dialog.dialog("close");
                    //var tid = parseInt($(".tab").last().attr("id").replace("tab", "")) + 1,
                    var tid = parseInt($('#menu li').length);

                    li = $("<li/>", {
                        id: "li" + tid,
                        class: "tab"
                    }).insertBefore("#menu li:last");
                    $("<a/>", {
                        text: $("#new_tab_input").val(),
                        href: "#tab" + tid,
                        id: "a" + tid
                    }).appendTo(li);

                    var groupName = $("#new_tab_input").val();
                    //API Call for CreateGroup
                    var model = { userName: "@UserName", groupName: groupName };
                    $.post("/UserGroup/CreateGroup", model, function (res) {
                        location.reload();
                    });
                    var tid = parseInt($('#menu li').length);
                    if (tid >= @id) {
                        $("#create_tab").hide();
}
                        },
    Cancel: function () {
        $dialog.dialog("close");
    }
                    },
    open: function () {
        $("#new_tab_input").val("");
    }
                });

$("#create_tab").click(function () {
    $dialog.dialog("open");
});

var OldGroupId = '';
var NewGroupId = '';
var ApplicaitonId = '';
$('.connectedSortable').on('click', 'input', function () {
    $(this).parent().toggleClass('selected');
});
$("#sortable1, #sortable2,#sortable3,#sortable4,#sortable5").sortable({
    //revert: 1,
    helper: function (e, item) { //create custom helper
        if (!item.hasClass('selected')) item.addClass('selected');
        // clone selected items before hiding
        var elements = $('.selected').not('.ui-sortable-placeholder').clone();
        //hide selected items
        item.siblings('.selected').addClass('hidden');
        var helper = $('<ul/>');
        return helper.append(elements);
    },

    start: function (e, ui) {
        var elements = ui.item.siblings('.selected.hidden').not('.ui-sortable-placeholder');
        //store the selected items to item being dragged
        ui.item.data('items', elements);
    },
    update: function (e, ui) {
        //manually add the selected items before the one actually being dragged
        ui.item.before(ui.item.data('items'));
    },
    stop: function (e, ui) {
        //show the selected items after the operation
        ui.item.siblings('.selected').removeClass('hidden');
        //unselect since the operation is complete
        $('.selected').removeClass('selected');
        //old active tab
        var activeTabIdx = $('#tabs').tabs('option', 'active');
        var selector = '#tabs > ul > li';
        OldGroupId = $(selector).eq(activeTabIdx).attr('data-id');
    }

}).disableSelection();
var $tabs = $("#tabs").tabs();
var $tab_items = $("ul:first li", $tabs).droppable({

    accept: "ul, .connectedSortable li",
    hoverClass: "ui-state-hover",
    drop: function (event, ui) {

        var $item = $(this);
        //New Group ID
        NewGroupId = $(this).attr('data-id');
        var $elements = ui.draggable.data('items');
        var $list = $($item.find("a").attr("href"))
            .find(".connectedSortable");
        $elements.show().hide('slow');
        ui.draggable.show().hide("slow", function () {
            // Application Id
            ApplicaitonId = $(this).attr('data-id');

            //API call for UpdateUserGroupApplication
            var model = { oldGroupId: OldGroupId, newGroupId: NewGroupId, applicationId: ApplicaitonId };

            $.post("/UserGroup/UpdateApplicationGroup", model, function (res) {
                // location.reload();
            });
            $tabs.tabs("option", "active", $tab_items.index($item));
            $(this).appendTo($list).show("slow").before($elements.show("slow"));
        });
    }
});

var count = '';
var GroupName = '';

$('#menu li').not(":last").on("click", "span", function () {
    var tabname = $('#a' + count).text();
    // alert(tabname);
    var model = { userName: "@UserName", groupName: tabname };
    if ($('#sortable' + count + '>li').length > 0) {

        alert("Tab contains applications, ​You can't delete.");
    }
    else {
        $.post("/UserGroup/DeleteGroup", model, function (res) {
            location.reload();
        });
    }
});
$.ajax({
    url: "/UserGroup/GetUserGroup",
    type: "GET",
    data: { userName: "@UserName", groupName: "ALL" },
    success: function (response) {
        $ulSub1 = $("#sortable1").empty();
        $.each(response, function (i, item) {
            $ulSub1.append('<li class="ui-state-default" data-id="' + item.applicationId + '"><a href="' + item.applicationUrl + '"><img class="zoom" style="height:70px;width:90px" src="/images/dashboard/' + item.applicationIcon + '" /></a></li>');
        });
    },
    error: function () {
    }
});
$('#menu li').not(":last").on("click ", function () {

    count = parseInt($(this).index()) + 1;
    GroupName = $('#a' + count).text();

    //API call For getGroupApplication
    $.ajax({
        url: "/UserGroup/GetUserGroup",
        type: "GET",
        data: { userName: "@UserName", groupName: GroupName },
        success: function (response) {
            //alert(JSON.stringify(response));
            $ulSub1 = $("#sortable1").empty();
            $ulSub2 = $("#sortable2").empty();
            $ulSub3 = $("#sortable3").empty();
            $ulSub4 = $("#sortable4").empty();
            $ulSub5 = $("#sortable5").empty();

            $.each(response, function (i, item) {
                if (count == 1) {
                    $ulSub1.append(
                        '<li   class="ui-state-default" data-id="' + item.applicationId + '"><a href="' + item.applicationUrl + '"><img class="zoom" style="height:70px;width:90px" src="/images/dashboard/' + item.applicationIcon + '" /></a></li>');
                }
                else if (count == 2) {
                    $ulSub2.append(
                        '<li class="ui-state-default"  data-id="' + item.applicationId + '"><a href="' + item.applicationUrl + '"><img class="zoom" style="height:70px;width:90px" src="/images/dashboard/' + item.applicationIcon + '" /></a></li>');
                }
                else if (count == 3) {
                    $ulSub3.append(
                        '<li class="ui-state-default"  data-id="' + item.applicationId + '"><a href="' + item.applicationUrl + '"><img class="zoom" style="height:70px;width:90px" src="/images/dashboard/' + item.applicationIcon + '" /></a></li>');
                }
                else if (count == 4) {
                    $ulSub4.append(
                        '<li class="ui-state-default"  data-id="' + item.applicationId + '"><a href="' + item.applicationUrl + '"><img class="zoom" style="height:70px;width:90px" src="/images/dashboard/' + item.applicationIcon + '" /></a></li>');
                }
                else if (count == 5) {
                    $ulSub5.append(
                        '<li class="ui-state-default"  data-id="' + item.applicationId + '"><a href="' + item.applicationUrl + '"><img class="zoom" style="height:70px;width:90px" src="/images/dashboard/' + item.applicationIcon + '" /></a></li>');
                }
            });
        },
        error: function () {
        }
    });
});
var tid1 = parseInt($('#menu li').length);
if (tid1 > @id)
{
    $("#create_tab").hide();
}
        });