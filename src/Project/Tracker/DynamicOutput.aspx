<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DynamicOutput.aspx.cs" Inherits="Tracker.DynamicOutput" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            
            <asp:UpdatePanel runat="server" ID="pnlOutput">
                <ContentTemplate>
                    <asp:Button runat="server" ID="btnRefresh" OnClick="btnRefresh_OnClick" Text="Refresh"/>
                    
                    <div>
                        <ul>
                            <asp:Repeater runat="server" ID="rptData" OnItemDataBound="rptData_OnItemDataBound">
                                <ItemTemplate>
                                    <li>
                                        <asp:Literal runat="server" ID="litTitle"></asp:Literal>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
