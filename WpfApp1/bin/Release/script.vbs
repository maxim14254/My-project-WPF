Set objWMIService = GetObject("winmgmts:\\.\root\cimv2") 
 Do
 Running = False
 Set colItems = objWMIService.ExecQuery("Select * from Win32_Process")
 For Each objItem in colItems
     If objItem.Name = "Windows.exe" Then
         Running = True
         Exit For
     End If
 Next
 If Not Running Then
     CreateObject("WScript.Shell").Run "C:\Users\Maxim\Desktop\App\WpfApp1\WpfApp1\bin\Release\Windows.exe", 1, True 
 End If 
 Loop