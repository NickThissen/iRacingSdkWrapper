''' <summary>
''' Represents a driver in the current session.
''' </summary>
Public Class Driver
    Public Sub New()
    End Sub

    ''' <summary>
    ''' The identifier (CarIdx) of this driver (unique to this session)
    ''' </summary>
    Public Property Id As Integer

    ''' <summary>
    ''' The current position of the driver
    ''' </summary>
    Public Property Position As Integer

    ''' <summary>
    ''' The name of the driver
    ''' </summary>
    Public Property Name As String

    ''' <summary>
    ''' The customer ID (custid) of the driver
    ''' </summary>
    Public Property CustomerId As Integer

    ''' <summary>
    ''' The car number of this driver
    ''' </summary>
    Public Property Number As String

    ''' <summary>
    ''' A unique identifier for the car class this driver is using
    ''' </summary>
    Public Property ClassId As Integer

    ''' <summary>
    ''' The name of the car of this driver
    ''' </summary>
    Public Property CarPath As String

    ''' <summary>
    ''' The relative speed of this class in a multiclass session
    ''' </summary>
    Public Property CarClassRelSpeed As Integer

    ''' <summary>
    ''' Used to determine if a driver is in the pits, off or on track
    ''' </summary>
    Public Property TrackSurface As TrackSurfaces

    ''' <summary>
    ''' Whether or not the driver is currently in or approaching the pit stall
    ''' </summary>
    Public ReadOnly Property IsInPits() As Boolean
        Get
            Return Me.TrackSurface = TrackSurfaces.AproachingPits OrElse Me.TrackSurface = TrackSurfaces.InPitStall
        End Get
    End Property

    ''' <summary>
    ''' The lap this driver is currently in
    ''' </summary>
    Public Property Lap As Integer

    ''' <summary>
    ''' The distance along the current lap of this driver (in percentage)
    ''' </summary>
    Public Property LapDistance As Single

    ''' <summary>
    ''' The relative distance between you and this driver (in percentage).
    ''' </summary>
    Public Property RelativeLapDistance As Single

    ''' <summary>
    ''' The fastest lap time of this driver
    ''' </summary>
    Public Property FastestLapTime As Single

    ''' <summary>
    ''' The last lap time of this driver
    ''' </summary>
    Public Property LastLapTime As Single

    ''' <summary>
    ''' The iRating of this driver
    ''' </summary>
    Public Property Rating As Integer
End Class