﻿<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
		<link rel="shortcut icon" href="../favicon.ico" type="image/x-icon"/>
        <link rel="stylesheet" href="css/style.css" type="text/css" media="screen"/>
        <style>
			body{
				background:#333 url(bg.jpg) repeat top left;
				font-family:Arial;
			}
			span.reference{
				position:fixed;
				left:10px;
				bottom:10px;
				font-size:12px;
			}
			span.reference a{
				color:#aaa;
				text-transform:uppercase;
				text-decoration:none;
				text-shadow:1px 1px 1px #000;
				margin-right:30px;
			}
			span.reference a:hover{
				color:#ddd;
			}
			ul.sdt_menu{
				margin-top:150px;
			}
		</style>
    </head>

    <body>
		<div class="content">
			<h1 class="title">Slide Down Box Menu with jQuery and CSS3</h1>
			<ul id="sdt_menu" class="sdt_menu">
				<li>
					<a href="#">
						<img src="images/2.jpg" alt=""/>
						<span class="sdt_active"></span>
						<span class="sdt_wrap">
							<span class="sdt_link">About me</span>
							<span class="sdt_descr">Get to know me</span>
						</span>
					</a>
				</li>
				<li>
					<a href="#">
						<img src="images/1.jpg" alt=""/>
						<span class="sdt_active"></span>
						<span class="sdt_wrap">
							<span class="sdt_link">Portfolio</span>
							<span class="sdt_descr">My work</span>
						</span>
					</a>
					<div class="sdt_box">
							<a href="#">Websites</a>
							<a href="#">Illustrations</a>
							<a href="#">Photography</a>
					</div>
				</li>
				<li>
					<a href="#">
						<img src="images/3.jpg" alt=""/>
						<span class="sdt_active"></span>
						<span class="sdt_wrap">
							<span class="sdt_link">Inspiration</span>
							<span class="sdt_descr">Where ideas get born</span>
						</span>
					</a>
				</li>
				<li>
					<a href="#">
						<img src="images/4.jpg" alt=""/>
						<span class="sdt_active"></span>
						<span class="sdt_wrap">
							<span class="sdt_link">Photos</span>
							<span class="sdt_descr">I like to photograph</span>
						</span>
					</a>
				</li>
				<li>
					<a href="#">
						<img src="images/5.jpg" alt=""/>
						<span class="sdt_active"></span>
						<span class="sdt_wrap">
							<span class="sdt_link">Blog</span>
							<span class="sdt_descr">I write about design</span>
						</span>
					</a>
				</li>
				<li>
					<a href="#">
						<img src="images/6.jpg" alt=""/>
						<span class="sdt_active"></span>
						<span class="sdt_wrap">
							<span class="sdt_link">Projects</span>
							<span class="sdt_descr">I like to code</span>
						</span>
					</a>
					<div class="sdt_box">
						<a href="#">Job Board Website</a>
						<a href="#">Shopping Cart</a>
						<a href="#">Interactive Maps</a>
					</div>
				</li>
			</ul>
		</div>
        <div>
            <span class="reference">
                <a href="http://tympanus.net/codrops/2010/07/16/slide-down-box-menu/">back to the Codrops Tutorial</a>
				<a href="http://www.flickr.com/photos/arcticpuppy/sets/72157622090180990/">Images by tibchris</a>
            </span>
		</div>

        <!-- The JavaScript -->
        <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
		<script type="text/javascript" src="jquery.easing.1.3.js"></script>
        <script type="text/javascript">
            $(function() {
				/**
				* for each menu element, on mouseenter, 
				* we enlarge the image, and show both sdt_active span and 
				* sdt_wrap span. If the element has a sub menu (sdt_box),
				* then we slide it - if the element is the last one in the menu
				* we slide it to the left, otherwise to the right
				*/
                $('#sdt_menu > li').bind('mouseenter',function(){
					var $elem = $(this);
					$elem.find('img')
						 .stop(true)
						 .animate({
							'width':'170px',
							'height':'170px',
							'left':'0px'
						 },400,'easeOutBack')
						 .andSelf()
						 .find('.sdt_wrap')
					     .stop(true)
						 .animate({'top':'140px'},500,'easeOutBack')
						 .andSelf()
						 .find('.sdt_active')
					     .stop(true)
						 .animate({'height':'170px'},300,function(){
						var $sub_menu = $elem.find('.sdt_box');
						if($sub_menu.length){
							var left = '170px';
							if($elem.parent().children().length == $elem.index()+1)
								left = '-170px';
							$sub_menu.show().animate({'left':left},200);
						}	
					});
				}).bind('mouseleave',function(){
					var $elem = $(this);
					var $sub_menu = $elem.find('.sdt_box');
					if($sub_menu.length)
						$sub_menu.hide().css('left','0px');
					
					$elem.find('.sdt_active')
						 .stop(true)
						 .animate({'height':'0px'},300)
						 .andSelf().find('img')
						 .stop(true)
						 .animate({
							'width':'0px',
							'height':'0px',
							'left':'85px'},400)
						 .andSelf()
						 .find('.sdt_wrap')
						 .stop(true)
						 .animate({'top':'25px'},500);
				});
            });
        </script>
    </body>
</html>