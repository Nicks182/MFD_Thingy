
var G_IsFullscreen = false;

function _SetFullscreen()
{
	if (G_IsFullscreen == true)
	{
		document.exitFullscreen()
		G_IsFullscreen = false;
	}
	else
	{
		
		var element = document.documentElement;
		if (element.requestFullscreen) element.requestFullscreen();
		else if (element.mozRequestFullScreen) element.mozRequestFullScreen();
		else if (element.webkitRequestFullscreen) element.webkitRequestFullscreen();
		else if (element.msRequestFullscreen) element.msRequestFullscreen();
		G_IsFullscreen = true;
	}
}
