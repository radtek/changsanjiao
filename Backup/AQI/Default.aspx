<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="AQI_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<title>�ۺ�Ԥ��</title>
 <script language="javascript" type="text/javascript">
//        <%if (m_UnLogin) {%>
//            top.location.href = "../Default.aspx";
//        <%} %>
        var lastTab = "<%=m_FirstTab %>";//��ǰѡ�е���Ⱦ��
        var userJson = "<%=m_UserJson %>";
        var peopleJson="<%=m_PeopleJson %>";
        var dianxinUser="<%=m_dianxinUser %>";
        var userLimit="<%=Limits %>";
        var userName="<%=LoginName %>";
        
 </script>
 
 <link href="images/css/css.css" rel="stylesheet" type="text/css" />
 <link type="text/css" rel="stylesheet" href="../Ext/resources/css/ext-all.css"/>
 <script type="text/javascript" src="../Ext/adapter/ext/ext-base.js"></script>
 <script type="text/javascript" src="../Ext/ext-all.js"></script>
 <script type="text/javascript" src="../Ext/ext-lang-zh_CN.js"></script>
 
 <script language="javascript" type="text/javascript" src="js/AQIUtility.js"></script>
 <script language="javascript" type="text/javascript" src="../JS/Utility.js"></script>
 <script language="javascript" type="text/javascript" src="js/AQI.js"></script>
 <script language="javascript" type="text/javascript" src="js/DataShare.js"></script>
 <script language ="javascript" type="text/javascript" src="../JS/highlight-active-input.js"> </script>
 <script language="javascript" type="text/javascript" src="../DatePicker/WdatePicker.js"></script>

</head>
<body onclick="hide(event)" style="-webkit-overflow-scrolling:touch; overflow: auto;">
        <div id="title">
            <div id="title1h">�Ϻ��п�������Ԥ��</div>
            <div id="titleDate"><%=m_ForecastDate %></div>
            <input id="nowDateTime" type="hidden" />
        </div>
    <div id="comForecast" class="show content">
       <div id="tabbtn" style="width: 1030px">
           <ul id="tabItem" runat="server">
           </ul>
         <div class="table-select">
         <button type="button" class="normal-btn table-select-btn"  id="yesDay"  onclick="changeDateSelect(-1)" onmouseover="this.className='normal-btn-h table-select-btn'" onmouseout="this.className='normal-btn table-select-btn'" onmousedown="this.className='normal-btn-d table-select-btn'" onmouseup ="this.className='normal-btn table-select-btn'">
            <span class="select-back"></span>
            <span class="select-text">��һ��</span>
         </button>
         <button type="button" class="normal-btn table-select-btn"  id="toDay"  onclick="today()" onmouseover="this.className='normal-btn-h table-select-btn'" onmouseout="this.className='normal-btn table-select-btn'" onmousedown="this.className='normal-btn-d table-select-btn'" onmouseup ="this.className='normal-btn table-select-btn'">
            <span class="select-now"></span>
            <span class="select-text">����</span>
         </button>
         <button type="button" class="normal-btn table-select-btn" id="tomoDay"  onclick="changeDateSelect(1)" onmouseover="this.className='normal-btn-h table-select-btn'" onmouseout="this.className='normal-btn table-select-btn'" onmousedown="this.className='normal-btn-d table-select-btn'" onmouseup ="this.className='normal-btn table-select-btn'">
            <span class="select-text">��һ��</span>
            <span class="select-next"></span>
         </button>
      </div>
      <div  style="clear:both" ></div>
        </div>  
        <div id="content" style="width: 1046px">
          <div id="tool" style="width: 1038px">
                <div class="selectDate"><input id="H00" runat="server" type="text" class="selectDateFormStyle" onchange="changeDate(this)" onclick="WdatePicker({dateFmt:'yyyy��MM��dd��'})"/></div>
                <div class="checkStyle">
                     <div class="checkLable">�ۺ�Ԥ����</div>
                     <div id="rd1" class="radioChecked"><a href="javascript:radioClick('rd1');">24h</a></div>
                     <div id="rd2" class="radioUnChecked"><a href="javascript:radioClick('rd2');">48h</a></div>
                </div>
                <div class="checkStyle">
                     <div class="checkLable">��ֵģʽ��</div>
                     <div id="rd3" class="radioUnChecked"><a href="javascript:radioClick('rd3');">CMAQ</a></div>
                     <div id="rd4"class="radioChecked"><a href="javascript:radioClick('rd4');">WRF</a></div>
                </div>
                  <div id="filter" class="lay" style="display:none">
                  </div>
                <div class="user">
                     <ul >
                         <li>���ࣺ<input id="H02" type="text" class="onlyInput" onclick = "Select(this,102)"  onkeyup= "Select(this,102)"/></li>
                         <li>���ࣺ<input id="H01" type="text" class ="onlyInput" onclick = "Select(this,102)" onkeyup="Select(this,102)"/></li>
                     </ul>
                </div>
                <div class="user">
                     <ul>
                         <li>�������ࣺ<input id="H04" type="text" class="onlyInput" onclick = "Select(this,202)" onkeyup="Select(this,202)" /></li>
                         <li>���󸱰ࣺ<input id="H03" type="text" class="onlyInput" onclick = "Select(this,201)"  onkeyup="Select(this,201)" /></li>
                     </ul>
                </div>
           </div>
           <div class="tableS">
          <table id="forecastTable" width="100%" border="0" cellpadding="0" cellspacing="0" class="tablekuang"  runat="server" >
        </table>
           </div>
        </div>
        <div  class="bottomDiv">
           <div class="tableS">
               <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tablekuang">
          <tr>
            <td class="tabletitle2">Ԥ��ʱ��</td>
            <td class="tabletitle3">24СʱԤ��</td>
            <td class="tabletitle3">48СʱԤ��</td>
          </tr>
          <tr>
            <td class="tablerow3">������������</td>
            <td class="tablerow3">
              <textarea id="H05" cols="45" rows="3" class="textarea" onchange="textAreaChange(this)"></textarea></td>
            <td class="tablerow3">
             <textarea id="H06" cols="45" rows="3" class="textarea" onchange="textAreaChange(this)"></textarea></td>
          </tr>
          <tr>
            <td class="tablerow3">��Ⱦ���̷���</td>
            <td class="tablerow3" ><textarea id="H07" cols="45" rows="3" class="textarea" onchange="textAreaChange(this)"></textarea></td>
            <td class="tablerow3"><textarea id="H08" cols="45" rows="3" class="textarea" onchange="textAreaChange(this)"></textarea></td>
          </tr>
          <tr>
            <td class="tablerow3">��������</td>
            <td class="tablerow3"  colspan="2"><textarea id="H11" cols="45" rows="3" class="textarea" onchange="textAreaChange(this)" style="width:890px;"></textarea></td>
          </tr>          <tr>
            <td class="tablerow3">Ԥ��Ԥ��</td>
            <td class="tablerow3" ><textarea id="H09" cols="45" rows="3" class="textarea" onchange="textAreaChange(this)"></textarea></td>
            <td class="tablerow3" ><textarea id="H10" cols="45" rows="3" class="textarea" onchange="textAreaChange(this)"></textarea></td>
          </tr>
        </table>
          </div>
        </div>
        <div class="btnarea">
               <input type="button"  id="btnSave" class="normal-btn input-btn" value="����" onclick="doSubmit(true);Broadcast(['H09','H10','PH09']);" onmouseover="this.className='normal-btn-h input-btn'" onmouseout="this.className='normal-btn input-btn'" onmousedown="this.className='normal-btn-d input-btn'" onmouseup ="this.className='normal-btn input-btn'"/>
              <input type="button" id="btnSubmit" class="normal-btn input-btn" value="�ύ" onclick="createSummary(true);Broadcast(['H09','H10','PH09']);" onmouseover="this.className='normal-btn-h input-btn'" onmouseout="this.className='normal-btn input-btn'" onmousedown="this.className='normal-btn-d input-btn'" onmouseup ="this.className='normal-btn input-btn'" />
        </div>
    </div>
    <div id="preview" class="hidden">
        <div id="content_huizong">
          <div  class="bottomDiv" >
             <ul>
                 <li class="userLiPre">�������</li>
                 <li class="userLiPreSmall">���ࣺ<span id="PH02"></span></li>
                 <li class="userLiPreSmall" style="padding-right: 35px">���ࣺ<span id="PH01"></span></li>
                 <li class="userLiPre">����</li>
                 <li class="userLiPreSmall">���ࣺ<span id="PH04"></span></li>
                 <li class="userLiPreSmall">���ࣺ<span id="PH03"></span></li>
             </ul>
           </div>
           <div id="table">
           <table id="pTable" width="100%" border="0" cellpadding="0" cellspacing="0" class="tablekuang" runat="server">
          <tr>
            <td class="tabletitle">Ԥ��ʱ��</td>
            <td class="tabletitleDate">����</td>
            <td class="tabletitle">&nbsp;</td>
            <td class="tabletitle">PM<sub>2.5</sub></td>
            <td class="tabletitle">PM<sub>10</sub></td>
            <td class="tabletitle">NO<sub>2</sub></td>
            <td class="tabletitle">O<sub>3</sub>-1h</td>
            <td class="tabletitle">O<sub>3</sub>-8h</td>
            <td class="tabletitle">AQI</td>
            </tr>
        </table>
           </div>
        </div>
        <div>
        <div class="btnarea">
               <input type="button"  id="sendSM" class="normal-btn input-btn" value="���ݷ���" onclick="changeEditor()" onmouseover="this.className='normal-btn-h input-btn'" onmouseout="this.className='normal-btn input-btn'" onmousedown="this.className='normal-btn-d input-btn'" onmouseup ="this.className='normal-btn input-btn'"/>
               <input type="button" id="goBack" class="normal-btn input-btn" value="����" onclick="goBack()" onmouseover="this.className='normal-btn-h input-btn'" onmouseout="this.className='normal-btn input-btn'" onmousedown="this.className='normal-btn-d input-btn'" onmouseup ="this.className='normal-btn input-btn'"/>
        </div>
        </div>

          <div id="DivMouse" class="layMouse" style="display:none">���������޶�
          </div>
    </div>
   <div id="ScanView" class="hidden">
    <div  class="bottomDiv" style="width: 900px">
    <label class="lableStyle" style="font-weight: bold">��ҳԤ��</label>
    <div class="tablekuang">
        <div id="webScan" style="padding-right: 5px; padding-bottom: 5px; padding-left: 5px">
        
        </div>
    </div>
    <label class="lableStyle" style="font-weight: bold">����΢��</label>
       <div class="tableS" style="width: 900px">
           <table width="100%" border="0" cellpadding="0" cellspacing="0" class="tablekuang">
                <tr>
                    <td class="tablerow6">����<br />
                      ����<br />
                      ����</td>
                    <td class="tablerow7">
                        <textarea id="PH09" cols="100" rows="2" class="textarea" onkeyup="textChange(this)" style="height: 60px; width: 830px;"></textarea>
                        <textarea id="PH10" cols="100" rows="2"  style="display:none;"></textarea>
                    </td>
                    
                    </tr>
          </table>
         <div id ="div1"  class="bottomDiv" style="width: 900px"> 
          <label style="margin-left: 5px">����ʱ��:&nbsp&nbsp<input type="text"  id="publishTime"  runat="server"  style="width: 150px" /></label>
         </div>
           <div id ="divbottom"  class="bottomDiv" style="width: 900px"> 
           <label class="checkSe" style="margin-right: 10px; margin-left: 35px;"><input type="checkbox"  checked ="checked"  name="Type"  id="message"  value ="0" />�ƶ��û�</label>
           <label id="re" class="checkSe" style="margin-left: 0px; text-decoration: underline; cursor: pointer;" onclick="receivePeople('block')">������</label>
            <label class="checkSe" style="margin-right: 10px;"><input type="checkbox" checked ="checked"   name="Type"  id="LTDX"  value ="5" />��ͨ����</label>
           <label id="DX" class="checkSe" style="margin-left: 0px; text-decoration: underline; cursor: pointer;" onclick="receivePeopleDX('block')">������</label>
           <label class="checkSe"><input type="checkbox" name="Type"  id="SHBJ"  checked ="checked" value ="1" />�л�����</label>
           <label class="checkSe"><input type="checkbox" name="Type"  id="XJZX"  value ="2" />��������</label>
           <label class="checkSe"><input  type="checkbox" name="Type" checked ="checked" id="Wb"  value ="4" />����΢��</label>
           <label class="checkSe"><input  type="checkbox" name="Type" checked ="checked" id="txWb"  value ="6" />��Ѷ΢��</label>
            <label class="checkSe"><input type="checkbox" name="Type"  id="ssfb"  checked ="checked" value ="3" />ʵʱ����ϵͳ</label>
           </div>
      </div>
    </div>
       <div  id="panGroupContainer" style="position: absolute; display: none;width: 400px; height: 250px; background-color: white; border: 2px solid rgb(193, 218, 215);">
         <div id="chkSiteContainer" style="width: 100%; height: 225px; overflow-y: scroll;border-bottom: solid 1px #C1DAD7">
        </div>
          <div style="padding-top: 2px;height:20px;">
            <div style="float: left;">
                &nbsp;&nbsp;
                <input type="button" class="button_default" value="ȫѡ" onclick="allSelecet('CheckType')"/>
                &nbsp;&nbsp;
                <input type="button" class="button_default" value="��ѡ" onclick="fanSelecet('CheckType')"/>
            </div>
            <div style="float: right">
                <input type="submit" name="btnSettingSites" value="ȷ��" onclick="OKSelecet()" id="btnSettingSites" class="button_default"/>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" value="�ر�" onclick="closeCheck('panGroupContainer')" class="button_default"/>
            </div>
        </div>

        </div>
         <div  id="panGroupContainerDX" style="position: absolute; display: none;width: 400px; height: 250px; background-color: white; border: 2px solid rgb(193, 218, 215);">
         <div id="chkSiteContainerDX" style="width: 100%; height: 225px; overflow-y: scroll;border-bottom: solid 1px #C1DAD7">
        </div>
          <div style="padding-top: 2px;height:20px;">
            <div style="float: left;">
                &nbsp;&nbsp;
                <input type="button" class="button_default" value="ȫѡ" onclick="allSelecet('CheckTypeDX')" />
                &nbsp;&nbsp;
                <input type="button" class="button_default" value="��ѡ" onclick="fanSelecet('CheckTypeDX')" />
            </div>
            <div style="float: right">
                <input type="submit" name="btnSettingSites" value="ȷ��" onclick="OKSelecetDX()" id="btnSettingSitesDX" class="button_default" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <input type="button" value="�ر�" onclick="closeCheck('panGroupContainerDX')" class="button_default" />
            </div>
        </div>

        </div>
        <div class="btnarea">
          <input type="button"  id="OKScan" class="normal-btn input-btn" value="ȷ��" onclick="OKScanEditor()" onmouseover="this.className='normal-btn-h input-btn'" onmouseout="this.className='normal-btn input-btn'" onmousedown="this.className='normal-btn-d input-btn'"  onmouseup ="this.className='normal-btn input-btn'"/>
           <input type="button" id="ScanBack" class="normal-btn input-btn" value="����" onclick="ScanGoBack()" onmouseover="this.className='normal-btn-h input-btn'" onmouseout="this.className='normal-btn input-btn'" onmousedown="this.className='normal-btn-d input-btn'"  onmouseup ="this.className='normal-btn input-btn'"/>
        </div>

</div>
    <div id="publicStateContainer"  class="publicState" style="display: none; border: 2px solid rgb(193, 218, 215);">
        <div id="publicState">
        </div>
         <input type="button"  onclick="closeQuxian()" class="quXClose" onmouseover="this.className = 'quXcloseHover';" onmouseout ="this.className ='quXClose';"   id="quxianClose" />
    </div>
    </body>
</html>
