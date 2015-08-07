$(function()    {
    $("#client-list-tabs").tabs();
    $.connection.hub.url = "http://localhost:5001/signalr";
    var brokerHub = $.connection.BrokerHub;
    window.tabCount = 1;

    $("#connect-button").button();
    $("#shutdown-broker").button();
    $("#connect-button").click(function() {
        $.connection.hub.start({ jsonp: true }).done(function() {
            $("#connectionStatus").html("Connected").css("color", "green");
        });
    });
    $("#shutdown-broker").click(function() {
        brokerHub.server.shutdownBroker();
    });

    var tabContentsSubscriptions = "<div><h2>Subscriptions</h2><table {0} class=\"subscriptions-table\"></table></div>";
    var tabContentsPackets = "<div><h2>Packets</h2><table {0} class=\"packets-table\"></table></div>";

    function addSubscription(clientId, topic) {
        var table = $(".subscriptions-table[data-client-id=\"" + clientId + "\"]");
        table.append("<tr data-topic=\"" + topic + "\"><td>" + topic + "</td></tr>");
    }

    function removeSubscription(clientId, topic) {
        var table = $(".subscriptions-table[data-client-id=\"" + clientId + "\"]");
        table.find("[data-topic=\"" + topic + "\"]").remove();
    }

    function addTab(client) {
        $("#client-tab-ul")
            .append($("<li><a data-client-id=\"" + client.UniqueId + "\" href=\"#client-" + window.tabCount + "\">" + client.ClientID + "</a></li>"));

        var thisTabContentSub = tabContentsSubscriptions.replace("{0}", "data-client-id=\"" + client.UniqueId + "\"");
        var thisTabContentPacket = tabContentsPackets.replace("{0}", "data-client-id=\"" + client.UniqueId + "\"");

        $("#client-list-tabs")
            .append($("<div id=\"client-" + window.tabCount + "\"><h3>" + client.ClientID + " (UID = " +client.UniqueId+ ")</h3><div><p>"
            + thisTabContentSub + thisTabContentPacket +
            "</p></div></div>"));
        window.tabCount++;
        $("#client-list-tabs").tabs("refresh");

        for(var i = 0; i< client.SubscriptionList.length;i++) 
            addSubscription(client.UniqueId, client.SubscriptionList[i]);
    };

    function removeTab(clientId) {
        var tabListHeader = $("a[data-client-id=\"" + clientId + "\"]");
        var tabToRemove = tabListHeader.attr("href");
        $(tabToRemove).remove();
        tabListHeader.parent().remove();
        $("#client-list-tabs").tabs("refresh");
    };

    function addPacket(recievedBool, clientId, mqttPacketType, data, time) {
        var table = $(".packets-table[data-client-id=\"" + clientId + "\"]");

        var recieveIconString = "<img src=\"Content/images/receive-icon.png\" >";
        var sendIconString = "<img src=\"Content/images/send-icon.png\" >";

        var iconString;
        if (recievedBool === true)
            iconString = recieveIconString;
        else 
            iconString = sendIconString;

        table.append(
            "<tr>" +
                "<td class=\"send-recieve-icon\">" +
                    iconString +
                "</td>" +
                "<td class=\"packet-type\">" +
                    mqttPacketType +
                "</td>" +
                "<td class=\"data-part\">" +
                    data +
                "</td>" +
                "<td>" +
                    time +
                "</td>" +
            "</tr>");
    }

    // setup signalr methods
    brokerHub.client.sendClientList = function (clientList) {
        for (var i = 0; i < clientList.length; i++) {
            var client = clientList[i].Value;
            addTab(client);
            $("#clients-summary-table").append(
                "<tr data-client-id=\"" + client.UniqueId + "\"><td>" + client.UniqueId + "</td><td>" + client.ClientID + "</td><td>" + client.IPAddress + "</td></tr>");
        }
    };

    brokerHub.client.newPacketRecieved = function (recievedBool, clientId, mqttPacketType, data, time) {
        addPacket(recievedBool, clientId, mqttPacketType, data, time);
    }


    brokerHub.client.newClientConnected = function (client) {
        addTab(client);
        $("#clients-summary-table").append(
            "<tr data-client-id=\""+client.UniqueId+"\"><td>"+ client.UniqueId +"</td><td>"+client.ClientID+"</td><td>"+client.IPAddress+"</td></tr>");
    };

    brokerHub.client.clientDisconnected = function (clientId) {
        removeTab(clientId);
        $("#clients-summary-table [data-client-id=\"" + clientId + "\"]").remove();
    };

    brokerHub.client.brokerDisconnecting = function () {
        $("#connectionStatus").html("Connection terminated by broker");
    };

    brokerHub.client.addSubscription = function (clientId, topic) {
        addSubscription(clientId, topic);
    }

    brokerHub.client.removeSubscription = function(clientId, topic) {
        removeSubscription(clientId, topic);
    }

    $.connection.hub.logging = true;
    $.connection.fn.log = function(msg) {
        $("#signalr-log-table").append("<tr><td>" + msg + "</td></tr>");
    };

    $.connection.hub.disconnected(function() {
        $("#connectionStatus").html("Not connected").css("color", "red");
        $("#connect-button").css("display", "inline-block");
    }).connectionSlow(function() {
        $("#connectionStatus").html("Connection is slow...").css("color", "orangered");
    });

    $.connection.hub.start({ jsonp: true }).done(function () {
        $("#connectionStatus").html("Connected").css("color", "green");
        $("#connect-button").css("display", "none");
    }).fail(function () {
        $("#connectionStatus").html("Connection to broker failed");
        $("#connect-button").css("display", "inline-block");
    });
});