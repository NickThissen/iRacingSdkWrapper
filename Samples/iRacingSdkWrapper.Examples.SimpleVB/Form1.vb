Imports System.Text
Imports iRacingSdkWrapper.Bitfields
Imports iRSDKSharp

Public Class Form1

    Private wrapper As SdkWrapper

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        ' Create a new instance of the SdkWrapper object
        wrapper = New SdkWrapper()

        ' Tell it to raise events on the current thread (don't worry if you don't know what a thread is)
        wrapper.EventRaiseType = SdkWrapper.EventRaiseTypes.CurrentThread

        ' Only update telemetry 10 times per second
        wrapper.TelemetryUpdateFrequency = 10

        ' Attach some useful events so you can respond when they get raised
        AddHandler wrapper.Connected, AddressOf wrapper_Connected
        AddHandler wrapper.Disconnected, AddressOf wrapper_Disconnected
        AddHandler wrapper.SessionInfoUpdated, AddressOf wrapper_SessionInfoUpdated
        AddHandler wrapper.TelemetryUpdated, AddressOf wrapper_TelemetryUpdated
    End Sub

    Private Sub startButton_Click(sender As System.Object, e As System.EventArgs) Handles startButton.Click
        ' If the wrapper is running, stop it. Otherwise, start it.
        If wrapper.IsRunning Then
            wrapper.Stop()
            startButton.Text = "Start"
        Else
            wrapper.Start()
            startButton.Text = "Stop"
        End If

        Me.StatusChanged()
    End Sub

    Private Sub StatusChanged()
        If wrapper.IsConnected Then
            If wrapper.IsRunning Then
                statusLabel.Text = "Status: connected!"
            Else
                statusLabel.Text = "Status: disconnected."
            End If
        Else
            If wrapper.IsRunning Then
                statusLabel.Text = "Status: disconnected, waiting for sim..."
            Else
                statusLabel.Text = "Status: disconnected."
            End If
        End If
    End Sub

    ' Event handler called when the sdk wrapper connects (eg, you start it, or the sim is started)
    Private Sub wrapper_Connected(ByVal sender As Object, ByVal e As EventArgs)
        Me.StatusChanged()
    End Sub

    ' Event handler called when the sdk wrapper disconnects (eg, the sim closes)
    Private Sub wrapper_Disconnected(ByVal sender As Object, ByVal e As EventArgs)
        Me.StatusChanged()
    End Sub

    ' Event handler called when the session info is updated
    ' This typically happens when a car crosses the finish line for example
    Private Sub wrapper_SessionInfoUpdated(ByVal sender As Object, ByVal e As SdkWrapper.SessionInfoUpdatedEventArgs)
        sessionInfoLabel.Text = String.Format("Session info (last update time: {0})", e.UpdateTime)

        ' Let's just dump the session info:
        sessionInfoTextBox.Text = e.SessionInfo.Yaml

        ' Read some YAML values using a YAML query
        Dim trackName As String = e.SessionInfo.GetValue("WeekendInfo:TrackName:")
        Dim sessionId As Integer = Integer.Parse(e.SessionInfo.GetValue("WeekendInfo:SessionId:"))
        Dim windSpeed As String = e.SessionInfo.GetValue("WeekendInfo:WeekendOptions:WindSpeed:")
        
        ' Read some values with the easier notation (no need to write the yaml query)
        Dim trackLength = e.SessionInfo("WeekendInfo")("TrackLength").GetValue()
        Dim driver1Name = e.SessionInfo("DriverInfo")("Drivers")("CarIdx", "1")("UserName").GetValue()

        ' Attempt to read the username of a random driver safely
        Dim random = New Random()
        Dim id = random.Next(0, 60)

        Dim driverName As String
        If e.SessionInfo("DriverInfo")("Drivers")("CarIdx", CStr(id))("UserName").TryGetValue(driverName) Then
            ' Success
            MessageBox.Show(driverName)
        Else
            ' not found
            MessageBox.Show("Error retrieving name of driver " & id)
        End If

    End Sub

    ' Event handler called when the telemetry is updated
    ' This happens (max) 60 times per second
    Private Sub wrapper_TelemetryUpdated(ByVal sender As Object, ByVal e As SdkWrapper.TelemetryUpdatedEventArgs)
        telemetryLabel.Text = String.Format("Telemetry (last updated {0})", DateTime.Now.TimeOfDay.ToString())

        ' Let's just write some random values:
        Dim sb As New StringBuilder()

        Me.TelemetryExample(sb, e)
        Me.ArrayExample(sb, e)
        Me.BitfieldsExample(sb, e)

        ' Get a (fictional) value that is not covered in the TelemetryInfo properties
        Dim fictionalObject = wrapper.GetTelemetryValue(Of Integer)("VariableName")
        Dim fictionalValue = fictionalObject.Value

        telemetryTextBox.Text = sb.ToString()
    End Sub

#Region " Simple telemetry examples "

    ' Example method that adds speed and roll values to the string builder
    Private Sub TelemetryExample(sb As StringBuilder, e As SdkWrapper.TelemetryUpdatedEventArgs)
        ' Important distinction: TelemetryInfo.Speed (for example) returns an object of type TelemetryValue. 
        ' This object contains more than just the value; also the unit, name and a description.
        ' To get just the value, you use the Value property.
        ' This goes for every property of the TelemetryInfo class.

        sb.AppendLine("Speed: " & e.TelemetryInfo.Speed.Value) ' Without unit
        sb.AppendLine("Speed: " & e.TelemetryInfo.Speed.ToString()) ' With unit
        sb.AppendLine("Roll: " & e.TelemetryInfo.Roll.ToString())
    End Sub

    ' Example method that adds some data such as your lap distance and track surface to the string builder
    Private Sub ArrayExample(sb As StringBuilder, e As SdkWrapper.TelemetryUpdatedEventArgs)
        ' Get your own CarIdx
        Dim myId As Integer = wrapper.DriverId

        ' Get the arrays you want
        Dim lapDistances As Single() = e.TelemetryInfo.CarIdxLapDistPct.Value
        Dim surfaces As TrackSurfaces() = e.TelemetryInfo.CarIdxTrackSurface.Value

        ' Your data is at your id index:
        Dim myLapDistance As Single = lapDistances(myId)
        Dim mySurface As TrackSurfaces = surfaces(myId)

        sb.AppendLine("My lap distance: " & myLapDistance)
        sb.AppendLine("My track surface: " & mySurface.ToString())
    End Sub

    ' Example method that adds some caution flags to the string builder if they are displayed in the sim
    Private Sub BitfieldsExample(sb As StringBuilder, e As SdkWrapper.TelemetryUpdatedEventArgs)
        ' The value of SessionFlags returns a SessionFlag object which contains information about all currently active flags
        ' Use the Contains method to check if it contains a specific flag.
        ' 
        ' EngineWarnings and CameraStates behave similarly.

        Dim flags As SessionFlag = e.TelemetryInfo.SessionFlags.Value
        If flags.Contains(SessionFlags.Black) Then
            sb.AppendLine("Black flag!")
        End If
        If flags.Contains(SessionFlags.Disqualify) Then
            sb.AppendLine("DQ")
        End If
        If flags.Contains(SessionFlags.Repair) Then
            sb.AppendLine("Repair")
        End If
        If flags.Contains(SessionFlags.Checkered) Then
            sb.AppendLine("Checkered")
        End If
    End Sub

#End Region

    Private Sub Form1_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        wrapper.Stop()
    End Sub
End Class
