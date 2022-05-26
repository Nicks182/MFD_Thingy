# MFD_Thingy
Web Based Multi-Function Display for Sim games... or any games. 

# Why?
This app simulates keyboard and mouse inputs. It is very similar to other apps like matricapp.com or gameglass.gg, but apps like this require you to install a app/service on your main machine and then another app/client on the device you wish to use. The client app will then send your input requests to the service on your main machine and will often just simulate a key press. 

I wanted to make something that does not require an app to be installed on my device as older phones or tablets sometimes can't run the apps required.

Both of those apps mentioned above offer features not available here. They both have a free tier and is worth checking out.

# What does this app do?
This uses a WPF desktop application with it's own web host to host a HTML page which you can browse to with any device that can run a browser and is connected to your local network. 
The WPF app controls the web host and also allows the user to configure what the Displays/Screens should look like.

By no means is this some sort of complete solution and was put together in less than a week. As such some things were setup with the idea of doing more, but I ended up not doing a lot of it. I may do more work on it in the future, but for now it's functional which is all I really wanted.

Devices you can use with this can be old phones, tablets, or even a raspberry pi with touch screen. Anything with a browser and a network interface.


# Use cases?
I made this to use in Star Citizen which is the only Sim style game that I play currently. However, since this app simulates mouse and keyboard inputs, you can make all sorts of screens. You can have a screen for Kodi, youtube, or any app that is usable with shortcut keys.

# How can you get it?
Download the zip in the Build folder. It's a self contained build, thus the size, so you should not have to install things like dotnet framework.


# Demo
[![Youtube Demo](https://img.youtube.com/vi/yC3m9ijjm_Q/hqdefault.jpg)](https://www.youtube.com/watch?v=yC3m9ijjm_Q)



NOTES: 
- Uses https://github.com/HavenDV/H.InputSimulator

- I know some of the keys are impossible to find. This is due to the library that I'm using to simulate the keys. I used the list of keys that comes with the library as is. I may change this in the future.

- Uses a visual studio plugin called Bundle & Minifier by Mark Kristensen to bundle the CSS and javascript.
