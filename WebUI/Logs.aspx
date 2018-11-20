<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Logs.aspx.cs" Inherits="Logs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link href="css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
    function MoveOver(obj){
        for(i=0;i<obj.cells.length;i++){
            obj.cells(i).style.backgroundColor ="#deeeff";
            //obj.cells(i).className = "mouseover-bg";
            
        }   
    }

    function MoveOut(obj){
        for(i=0;i<obj.cells.length;i++){
            obj.cells(i).style.backgroundColor = "";//使得背景没有效果
            //obj.cells(i).className = "listtd";
        }
    }

    function SelectRow(obj){
        if (selectedRow != null) {
	        selectedRow.style.backgroundColor = lastColor;
        }
        selectedRow = obj;
        lastColor = selectedRow.style.backgroundColor;
        selectedRow.style.backgroundColor = "#BADBFF";
        
    }
    </script>
</head>
<body style="margin:0">
    <form id="form1" runat="server">
    <div>

<asp:GridView id="gdvQueryAttribute" runat="server" PageSize="15" Width="100%" ForeColor="#333333" Font-Size="12px" Font-Names="宋体" CellPadding="4">
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"  />
                                    <RowStyle BackColor="#EFF3FB" Wrap="False" HorizontalAlign="Center"  />
                                    <EditRowStyle BackColor="#2461BF"  />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333"  />
                                    <PagerStyle BackColor="White" ForeColor="#1A1A6B" HorizontalAlign="Left"  />
                                    <HeaderStyle BackColor="#507CD1" CssClass="Student" Font-Bold="True" ForeColor="#1A1A6B" Wrap="False"  />
                                    <AlternatingRowStyle BackColor="White"  />
                                   
                                </asp:GridView>
    </form>
</body>
</html>
