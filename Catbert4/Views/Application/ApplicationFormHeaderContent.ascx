<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<style type="text/css">
    .connectedSortable { list-style-type: none; margin: 0; padding: 0; margin-right: 10px; background: #eee; padding: 5px; width: 220px;}
	.connectedSortable li { margin: 5px; padding: 5px; font-size: 1.2em;  }
		
</style>

<script type="text/javascript">
    $(function () {
        $(".connectedSortable").sortable({
            connectWith: ".connectedSortable"
        }).disableSelection();

        $("form").submit(function (e) {
            e.preventDefault();

            var data = $(this).serializeArray();

            CollectRoleData(data);

            var jsonEditUrl = '<%: Request.Url %>';

            $.post(
                jsonEditUrl,
                data,
                function (result) {
                    if (result.success) {
                        window.location = '<%: Url.Action("Index") %>';
                    }
                },
                'json');
        });
    });

    //Get all of the roles, serialize them into string arrays and then push them into the data
    function CollectRoleData(data) {
        var ordered = $('#ordered-roles').sortable('toArray');
        var unordered = $('#unordered-roles').sortable('toArray');

        $(ordered).each(function (index, value) {
            data.push({ name: 'orderedRoles', value: value });
        });

        $(unordered).each(function (index, value) {
            data.push({ name: 'unorderedRoles', value: value });
        });
    }
</script>