﻿Imports Windows.UI.Core

''' <summary>
''' Provides application-specific behavior to supplement the default Application class.
''' </summary>
NotInheritable Class App
    Inherits Application

    ''' <summary>
    ''' Invoked when the application is launched normally by the end user.  Other entry points
    ''' will be used when the application is launched to open a specific file, to display
    ''' search results, and so forth.
    ''' </summary>
    ''' <param name="e">Details about the launch request and process.</param>
    Protected Overrides Sub OnLaunched(e As Windows.ApplicationModel.Activation.LaunchActivatedEventArgs)
#If DEBUG Then
        ' Show graphics profiling information while debugging.
        If System.Diagnostics.Debugger.IsAttached Then
            ' Display the current frame rate counters
            ' Me.DebugSettings.EnableFrameRateCounter = True
        End If
#End If

        Dim shell As Shell = TryCast(Window.Current.Content, Shell)

        ' Do not repeat app initialization when the Window already has content,
        ' just ensure that the window is active

        If shell Is Nothing Then
            ' Create a Frame to act as the navigation context and navigate to the first page
            shell = New Shell()

            ' hook-up shell root frame navigation events
            AddHandler shell.RootFrame.Navigated, AddressOf OnNavigated
            AddHandler shell.RootFrame.NavigationFailed, AddressOf OnNavigationFailed

            If e.PreviousExecutionState = ApplicationExecutionState.Terminated Then
                ' TODO: Load state from previously suspended application
            End If
            ' Place the frame in the current Window
            Window.Current.Content = shell

            ' listen for back button clicks (both soft- And hardware)
            AddHandler SystemNavigationManager.GetForCurrentView().BackRequested, AddressOf OnBackRequested

            UpdateBackButtonVisibility()
        End If

        ' Ensure the current window is active
        Window.Current.Activate()
    End Sub

    Private Sub OnBackRequested(sender As Object, e As BackRequestedEventArgs)
        Dim shell As Shell = TryCast(Window.Current.Content, Shell)
        If shell.RootFrame.CanGoBack Then
            e.Handled = True
            shell.RootFrame.GoBack()
        End If
    End Sub

    Private Sub OnNavigated(sender As Object, e As NavigationEventArgs)
        UpdateBackButtonVisibility()
    End Sub

    ''' <summary>
    ''' Invoked when Navigation to a certain page fails
    ''' </summary>
    ''' <param name="sender">The Frame which failed navigation</param>
    ''' <param name="e">Details about the navigation failure</param>
    Private Sub OnNavigationFailed(sender As Object, e As NavigationFailedEventArgs)
        Throw New Exception("Failed to load Page " + e.SourcePageType.FullName)
    End Sub

    ''' <summary>
    ''' Invoked when application execution is being suspended.  Application state is saved
    ''' without knowing whether the application will be terminated or resumed with the contents
    ''' of memory still intact.
    ''' </summary>
    ''' <param name="sender">The source of the suspend request.</param>
    ''' <param name="e">Details about the suspend request.</param>
    Private Sub OnSuspending(sender As Object, e As SuspendingEventArgs) Handles Me.Suspending
        Dim deferral As SuspendingDeferral = e.SuspendingOperation.GetDeferral()
        ' TODO: Save application state and stop any background activity
        deferral.Complete()
    End Sub

    Private Sub UpdateBackButtonVisibility()
        Dim shell As Shell = TryCast(Window.Current.Content, Shell)

        Dim visibility = AppViewBackButtonVisibility.Collapsed
        If shell.RootFrame.CanGoBack Then
            visibility = AppViewBackButtonVisibility.Visible
        End If

        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = visibility
    End Sub
End Class
