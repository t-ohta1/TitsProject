Public Class PagingTest

    Dim cn As SqlClient.SqlConnection
    Dim idList As New List(Of String)
    Dim nameList As New List(Of String)

    Private Sub PagingTest_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        connectDb()

        setData()

        createRadioAndText()


    End Sub

    Function getTotalData() As Integer

        Dim sdr As SqlClient.SqlDataReader

        Try

            Dim cmd As New SqlClient.SqlCommand
            Dim sqlStr As String

            cmd.Connection = cn
            cmd.CommandType = CommandType.Text

            sqlStr = "SELECT COUNT(CONSTELLATION_ID) FROM M_CONSTELLATION"

            cmd.CommandText = sqlStr

            sdr = cmd.ExecuteReader()

            cmd.Dispose()

            sdr.Read()

            Return Int(sdr(0).ToString())

        Catch ex As Exception

            MsgBox(ex.StackTrace())
            MsgBox(ex.Message())
            Throw New Exception()

        Finally

            sdr.Close()

        End Try

    End Function

    Sub createRadioAndText()

        Dim totalData As Integer = getTotalData()
        Dim radioVertical As Integer = 74
        Dim textVertical As Integer = 148
        Dim horizontal As Integer = 114

        If totalData > 5 Then


            Dim nextButton As New System.Windows.Forms.Button()
            Dim backButton As New System.Windows.Forms.Button()

            For i = 0 To 4

                Dim radio As New System.Windows.Forms.RadioButton
                Dim text As New System.Windows.Forms.TextBox

                radio.Text = idList(i)
                radio.Location = New System.Drawing.Point(radioVertical, horizontal)
                radio.Size = New System.Drawing.Size(33, 22)
                radio.Name = "add"

                text.Text = nameList(i)
                text.Location = New System.Drawing.Point(textVertical, horizontal)
                text.Size = New System.Drawing.Size(143, 25)
                text.Name = "add"

                Controls.Add(radio)
                Controls.Add(text)

                horizontal += 50

            Next

            nextButton.Text = ">>"
            nextButton.Location = New System.Drawing.Point(100, 300)
            nextButton.Name = "nextButton"

            backButton.Text = "<<"
            backButton.Location = New System.Drawing.Point(60, 300)
            backButton.Name = "backButton"

            Controls.Add(nextButton)
            Controls.Add(backButton)

            AddHandler nextButton.Click, AddressOf nextButton_Click
            AddHandler backButton.Click, AddressOf backButton_Click

        Else

            For i = 0 To totalData - 1

                Dim radio As New System.Windows.Forms.RadioButton
                Dim text As New System.Windows.Forms.TextBox

                radio.Text = idList(i)
                radio.Location = New System.Drawing.Point(radioVertical, horizontal)
                radio.Size = New System.Drawing.Size(33, 33)
                radio.Name = "id" & i

                text.Text = nameList(i)
                Text.Location = New System.Drawing.Point(textVertical, horizontal)
                Text.Size = New System.Drawing.Size(143, 25)
                text.Name = "name" & i

                Controls.Add(radio)
                Controls.Add(Text)

                horizontal += 50

            Next

        End If

    End Sub

    Sub displayNextData()

        Dim radioVertical As Integer = 74
        Dim textVertical As Integer = 148
        Dim horizontal As Integer = 134
        Dim nextButton As New System.Windows.Forms.Button
        Dim backButton As New System.Windows.Forms.Button

        For i = 5 To 9

            Dim radio As New System.Windows.Forms.RadioButton
            Dim text As New System.Windows.Forms.TextBox

            radio.Text = idList(i)
            radio.Location = New System.Drawing.Point(radioVertical, horizontal)
            radio.Size = New System.Drawing.Size(33, 22)
            radio.Name = "add"

            text.Text = nameList(i)
            text.Location = New System.Drawing.Point(textVertical, horizontal)
            text.Size = New System.Drawing.Size(143, 25)
            text.Name = "add"

            Controls.Add(radio)
            Controls.Add(text)

            horizontal += 50

        Next

    End Sub

    Private Sub nextButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        displayNextData()

    End Sub

    Private Sub backButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        displayNextData()

    End Sub

    Sub setData()

        Dim sdr As SqlClient.SqlDataReader
        Dim cmd As New SqlClient.SqlCommand
        Dim sqlStr As String


        cmd.Connection = cn
        cmd.CommandType = CommandType.Text

        sqlStr = "SELECT CONSTELLATION_ID, CONSTELLATION_NAME FROM M_CONSTELLATION ORDER BY CONSTELLATION_ID ASC"

        cmd.CommandText = sqlStr

        sdr = cmd.ExecuteReader()

        cmd.Dispose()

        While sdr.Read()

            idList.Add(sdr(0))
            nameList.Add(sdr(1))

        End While

        sdr.Close()

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
End Class