﻿Public Class MasterMaintenance

    Dim endType As Integer
    Dim textbox(5) As System.Windows.Forms.TextBox
    '0がユーザ、1が星座
    Dim maintenanceType As Integer

    Dim cn As System.Data.SqlClient.SqlConnection

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        endType = 1

        Me.Close()

    End Sub

    Private Sub MasterMaintenance_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed

        If endType = 1 Then

            MaintenanceMenu.Show()

            My.Application.ApplicationContext.MainForm = MaintenanceMenu

        ElseIf endtype = 0 Then

        End If
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

        closeConnection()

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


        Controls.Add(radio)
        Controls.Add(textbox)
        Controls.Add(password)


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

        createRadioAndTextBox()

        Button1.Text = "登録"

    End Sub
End Class