Public Class MasterMaintenance

    Dim endType As Integer
    Dim textbox(5) As System.Windows.Forms.TextBox
    '0がユーザ、1が星座
    Dim maintenanceType As Integer
    Dim adderText As System.Windows.Forms.TextBox
    Dim adderRadio As System.Windows.Forms.RadioButton
    Dim adderPass As System.Windows.Forms.TextBox

    Dim cn As System.Data.SqlClient.SqlConnection

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        endType = 1

        Me.Close()

    End Sub

    Private Sub MasterMaintenance_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed

        If endType = 0 Then

        ElseIf endType = 1 Then

            MaintenanceMenu.Show()

            My.Application.ApplicationContext.MainForm = MaintenanceMenu

        ElseIf endType = 2 Then

            DoneMaintenance.Show()

            My.Application.ApplicationContext.MainForm = DoneMaintenance

        End If

        closeConnection()

    End Sub

    Private Sub MasterMaintenance_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        createTextBoxList()
        connectDb()

        If Label2.Text = "ユーザー" Then

            maintenanceType = 0

        ElseIf Label2.Text = "星座" Then

            maintenanceType = 1

        End If

        setData()

    End Sub

    Sub connectDb()

        Try

            Dim serverName As String = "192.168.0.173"
            Dim dbName As String = "master"
            Dim userName As String = "sa"
            Dim password As String = "SaPassword2017"

            cn = New System.Data.SqlClient.SqlConnection()

            cn.ConnectionString =
                "Data Source = " & serverName &
                ";Initial Catalog = " & dbName &
                ";User ID = " & userName &
                ";Password = " & password

            cn.Open()

        Catch ex As Exception

            MsgBox(ex.Message)

        End Try

    End Sub

    Sub setData()

        Dim cmd As New SqlClient.SqlCommand

        cmd.Connection = cn
        cmd.CommandType = CommandType.Text

        Dim sr As SqlClient.SqlDataReader

        If maintenanceType = 0 Then

            cmd.CommandText = "SELECT G_ROW.USER_NAME FROM(
                                SELECT ROW_NUMBER() OVER(ORDER BY USER_ID ASC) AS ROW,
                                    USER_NAME
                                FROM M_USER)G_ROW
                                WHERE G_ROW.ROW BETWEEN 1 AND 4"

        ElseIf maintenanceType = 1 Then

            cmd.CommandText = "SELECT G_ROW.CONSTELLATION_NAME FROM(
                                SELECT ROW_NUMBER() OVER(ORDER BY CONSTELLATION_ID ASC) AS ROW,
                                    CONSTELLATION_NAME
                                FROM M_CONSTELLATION)G_ROW
                                WHERE G_ROW.ROW BETWEEN 1 AND 4"
        End If

        sr = cmd.ExecuteReader()

        cmd.Dispose()

        Dim i As Integer = 0
        While sr.Read

            textbox(i).Text = sr(0).ToString()
            i = i + 1

        End While

        sr.Close()

    End Sub

    Sub closeConnection()

        If cn.State <> ConnectionState.Closed Then

            cn.Close()
            cn.Dispose()

        End If
    End Sub

    Sub createTextBoxList()

        textbox(0) = TextBox1
        textbox(1) = TextBox2
        textbox(2) = TextBox3
        textbox(3) = TextBox4

    End Sub

    Sub createRadioAndTextBox()

        Dim textbox As New System.Windows.Forms.TextBox
        Dim password As New System.Windows.Forms.TextBox
        Dim radio As New System.Windows.Forms.RadioButton

        radio.Text = getTextBoxCnt(Me)
        radio.Name = "adderRadio"
        radio.Location = New System.Drawing.Point(74, 245)
        radio.Size = New System.Drawing.Size(33, 22)
        radio.TabIndex = 0

        textbox.Name = "adderTextBox"
        textbox.Location = New System.Drawing.Point(148, 242)
        textbox.Size = New System.Drawing.Size(143, 25)
        textbox.TabIndex = 0

        password.Name = "adderPassword"
        password.Location = New System.Drawing.Point(148, 273)
        password.Size = New System.Drawing.Size(143, 25)
        password.TabIndex = 0
        password.PasswordChar = "*"

        'formへの追加
        Controls.Add(radio)
        Controls.Add(textbox)
        Controls.Add(password)

        '値取得のため
        adderRadio = radio
        adderText = textbox
        adderPass = password


    End Sub

    Function getTextBoxCnt(ByVal ctrl As Control) As Integer

        If ctrl.Controls.Count = 0 Then
            If TypeOf ctrl Is TextBox Then

                Return 1

            Else

                Return 0
            End If
        End If
        Dim cnt As Integer

        For Each con As Control In ctrl.Controls

            cnt += getTextBoxCnt(con)

        Next

        Return cnt
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If Button1.Text = "追加" Then

            Button1.Text = "登録"

            createRadioAndTextBox()

        ElseIf Button1.Text = "登録" Then

            insertProcess()

        End If

    End Sub

    Sub insertProcess()

        Try

            Dim cmd As New SqlClient.SqlCommand
            Dim sqlStr As String
            Dim addId As String

            cmd.Connection = cn
            cmd.CommandType = CommandType.Text

            If adderRadio.Text.Length = 1 Then

                addId = "0" + adderRadio.Text

            Else

                addId = adderRadio.Text

            End If

            sqlStr = "INSERT INTO M_USER(USER_ID, USER_NAME, PASSWORD, DELETE_FLAG, CREATE_DATE, CREATE_USER)"
            sqlStr += "VALUES("
            sqlStr += "'" & addId & "',"
            sqlStr += "'" & adderText.Text & "',"
            sqlStr += "'" & adderPass.Text & "',"
            sqlStr += " 0, GETDATE(), '00')"

            cmd.CommandText = sqlStr

            cmd.ExecuteNonQuery()

            cmd.Dispose()

            endType = 2

            Me.Close()

        Catch ex As SqlClient.SqlException

            MsgBox(ex.Message())

        End Try

    End Sub
End Class