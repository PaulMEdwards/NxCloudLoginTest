# Automation Test
1. Visit http://nxvms.com/.
   Ensure browser is forwarded to https:// SSL encrypted login page.
   Ensure there are no errors in the browser console.
2. Click "Log In" button in top-right corner of page.
   Expected: "Log in to Nx Cloud" form displayed.
3. Login as test user with provided password.
   1. Input Email address & Password values into respective fields.
   2. Optionally check the "Remember me" box.
   3. Click "Log In"
4. Authentication OK, browser forwarded to Systems dashboard page.
   Expected: All registered systems on account are loaded & displayed.
   Ensure there are no errors in the browser console.
5. Logout and return to landing page.
   1. Click currently logged in user's Email address in top-right corner of page.
   2. Click "Log Out".


## To run, perform the following:
1. Install Visual Studio 2017.
2. Install SpecFlow for Visual Studio 2017 Extension.
3. Clone Repository to local machine.
4. Open Solution in Visual Studio 2017.
5. Enable the Test Explorer window.
   Menu: Test > Windows > Test Explorer
6. Build the Solution.
   NuGet packages should be automatically restored.
7. Might need to build again to see the tests within the Test Explorer.
8. Optionally, right-click the SpecFlowTestProject parent entry in the Test Explorer tree and select "Expand All".
9. Click Run All
10. View the Test Run Report file from the Tests Output pane.
11. Optionally, comment out or remove the "@Ignore" tag on the 2 "Bug" tests within the Dynamic section, then save the feature file and run each of those tests manually, viewing the Test Run Report file after each failure.
