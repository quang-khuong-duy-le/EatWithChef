$(document).ready(function() {
    $('#theme').change(function() {
        $('link:last').attr('href', 'css/timeline_' + $(this).val() + '.css?key=' + (new Date()).getTime());
    });

    $('#demo_type').change(function() {
        changeDemo(parseInt($(this).val(), 10));
    });

    $('#hashtag').keyup(function(e) {
        if (e.which === 13) {
            if ($.trim($('#hashtag').val()).replace(/#/, '') !== '') {
                twitterSearch();
            }
        }
    });

    $('#facebook_search').keyup(function(e) {
        if (e.which === 13) {
            if ($.trim($('#facebook_search').val()) !== '') {
                facebookSearch();
            }
        }
    });

    $('#rss_feed').keyup(function(e) {
        if (e.which === 13) {
            if ($.trim($('#rss_feed').val()) !== '') {
                readRssFeed();
            }
        }
    });

    $('#rss_feed_list').change(function(e) {
        readRssFeed($('#rss_feed_list').val());
    });

    $('#facebook_search').on('keydown', function() {
        $('#facebook_page_id').val('');
    });
    $('#facebook_page_id').on('keydown', function() {
        $('#facebook_search').val('');
    });

});

var currentFrame;
var currentElement;
function openFrame(id, thisElement) {
    currentFrame = id;
    currentElement = thisElement;
    $(thisElement).css("background-image", "url('/Content/frontend/timeline/images/loader.gif')");
    setTimeout(function () {

        $(".list-dish-chef").css("z-index", "10");
        $(".list-dish-chef").css("opacity", "1");
        $("body").css("overflow", "hidden");
        $("#content-detail").css("z-index", "2");

        setTimeout(function () {
            $("#detail-dish-" + id).css("opacity", "1");
            $("#detail-dish-" + id).css("z-index", "11");
            $("#detail-dish-" + id).css("margin-top", "-310px");
        }, 300);
        setTimeout(function () { moveImg(id); }, 500);
    }, 1200);
}

var timer;
function moveImg(id) {
    if($("#detail-dish-" + id + " > .content-img > img").position().left > -150){
        $("#detail-dish-" + id + " > .content-img > img").css("left", "-300px");
    } else{
        $("#detail-dish-" + id + " > .content-img > img").css("left", "0");
    }
    timer = setTimeout(function () { moveImg(id); }, 10000);
}

function closeFrame() {
    $(currentElement).css("background-image", "url('/Content/frontend/timeline/images/search.png')");

    $("#detail-dish-" + currentFrame).css("opacity", "0");
    $("#detail-dish-" + currentFrame).css("z-index", "-1");
    $("#detail-dish-" + currentFrame).css("margin-top", "-900px");

    $("#detail-dish-" + currentFrame + " > .content-img > img").css("left", "0");

    setTimeout(function () {
        $(".list-dish-chef").css("z-index", "-1");
        $(".list-dish-chef").css("opacity", "0");
        $("body").css("overflow", "scroll");
        $("#content-detail").css("z-index", "0");
    }, 300);


    clearTimeout(timer);
}

function loadMoreHow(id) {
    if ($("#how-to-it-" + id).height() == 0) {
        $("#how-to-it-" + id).css("height", "414px");
        $("#how-to-it-" + id).css("opacity", "1");
    } else {
        $("#how-to-it-" + id).css("height", "0");
        $("#how-to-it-" + id).css("opacity", "0");
    }
}

function changeDemo(type, is_mobile) {
    var datafortimeline = [];
    //ajax get timeline data (Nhan Nguyen)
    $.ajax({
        url: "/Chef/GetDishByChefId",
        type: "GET",
        async: false,
        data: { ChefId: 1 },
        success: function (data) {
            var chef = JSON.parse( data.Chef);
            var list = JSON.parse(data.List);

            for (var i = 0; i < list.length; i++) {
                var timelineitem = {
                    dishId: list[i].Id, 
                    type: 'blog_post',
                    date: '2011-08-03',
                    title: list[i].Name,
                    width: 400,
                    content: list[i].Description,
                    image: list[i].Image,
                    readmore: list[i].Url
                }
                datafortimeline.push(timelineitem);

                var detail = "<div class='chef-detail-dish' id='detail-dish-" + list[i].Id + "'>";
                        detail += "<div class='content-img'>";
                            detail += "<div class='like-btn'><input type='button' value='Tuyệt vời!'/></div>";
                            detail += "<img src='" + list[i].Image + "'/>";
                        detail += "</div>";
                        detail += "<div class='dish-detail'>";
                            detail += "<div class='dish-title'>";
                                detail += "<img src='/Content/frontend/images/team-img-1.jpg' />";
                                detail += "<p>" + list[i].Name + "</p><span> Đầu bếp " + chef.Name + "</span>";
                            detail += "</div>";
                            detail += "<div class='how-do-it' onclick='loadMoreHow(" + list[i].Id + ")'>Cách chế biến</div>";
                            detail += "<p>215 người yêu thích món ăn này</p>";
                            detail += "<div class='how-to-it-desc' id='how-to-it-" + list[i].Id + "'>";
                                detail += "<div>" + list[i].Description + "</div>";
                            detail += "</div>";
                            detail += "<div class='dish-description'>" + list[i].Description + "</div>";
                        detail += "</div>";
                    detail += "</div>";

                $("#list-dish-chef").append(detail);
            }
        }
    });


    $('#timeline').remove();
    var wrapper = $('<div>').attr('id', 'timeline').appendTo('#main');

    if (is_mobile) {
        wrapper.addClass('mobile');
    }

    var timeline_data = [];
    var options       = {};

    $('#timeline').addClass('demo' + type);

    switch (type) {
        case 1:
            timeline_data = datafortimeline;
            options       = {
                animation:   true,
                lightbox:    true,
                showYear:    true,
                allowDelete: false,
                columnMode:  'dual'
            };
            break;
        case 2:
            timeline_data = [
                {
                    type:     'blog_post',
                    date:     '2011-09-03',
                    title:    'FA Cup',
                    width:    300,
                    content:  'The Reds go marching on in the FA Cup...',
                    image:    'images/facup.jpg'
                },
                {
                    type:     'blog_post',
                    date:     '2011-08-03',
                    title:    'Swansea',
                    width:    300,
                    content:  'Check out our exclusive video preview ahead of today\'s clash with Swansea <a href="http://bit.ly/Yz0bmZ" target="_blank">http://bit.ly</a>',
                    image:    'images/rio.jpg'
                },
                {
                    type:     'blog_post',
                    date:     '2011-07-15',
                    title:    'Manchester United VS Liverpool',
                    width:    300,
                    content:  'The Reds complete the double over Liverpool this season...',
                    image:    'images/evra.jpg'
                },
                {
                    type:     'blog_post',
                    date:     '2011-06-29',
                    title:    'Michael Carrick',
                    width:    300,
                    content:  'Last chance to win Michael Carrick\'s signed shirt from the Liverpool game!! Click this link to enter <a href="http://bit.ly/W03U8k" target="_blank">http://bit.ly</a>',
                    image:    'images/carric.jpg'
                },
                {
                    type:     'blog_post',
                    date:     '2011-04-02',
                    title:    'Match',
                    width:    300,
                    content:  '9 Premier League wins out of 10 this season at Old Trafford. What is your match of the season so far at the Theatre of Dreams?',
                    image:    'images/wigan.jpg'
                },
                {
                    type:     'blog_post',
                    date:     '2011-02-13',
                    title:    'Old Traffordt',
                    width:    300,
                    content:  'Check out our exclusive video preview ahead of today\'s clash with Swansea <a href="http://bit.ly/Yz0bmZ" target="_blank">http://bit.ly</a>',
                    image:    'images/home.jpg'
                }
            ];
            options       = {
                animation:   true,
                lightbox:    true,
                showYear:    false,
                allowDelete: true,
                columnMode:  'dual'
            };
            break;
        case 3:
            timeline_data = [
                {
                    type:     'slider',
                    date:     '2011-12-16',
                    width:    400,
                    height:   150,
                    images:   ['images/group.jpg', 'images/old.jpg', 'images/win.jpg'],
                    speed:    5000
                },
                {
                    type:     'gallery',
                    date:     '2011-04-12',
                    title:    'Mini Gallery',
                    width:    400,
                    height:   100,
                    images:   ['images/rooney.jpg', 'images/tshirt.jpg', 'images/giggs.jpg', 'images/rio.jpg', 'images/paper.jpg']
                },
                {
                    type:     'blog_post',
                    date:     '2011-08-03',
                    title:    'Blog Post',
                    width:    400,
                    content:  '<b>Lorem Ipsum</b> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.',
                    image:    'images/rio.jpg',
                    readmore: 'http://www.manutd.com'
                },
                {
                    type:     'slider',
                    date:     '2010-12-16',
                    width:    400,
                    height:   200,
                    images:   ['images/ferguson.jpg', 'images/paper.jpg'],
                    speed:    5000
                },
                {
                    type:     'gallery',
                    date:     '2010-04-12',
                    title:    'Mini Gallery',
                    width:    400,
                    height:   150,
                    images:   ['images/stadium.jpg', 'images/rafel.jpg', 'images/logo.jpg', 'images/rvp.jpg']
                },
                {
                    type:     'blog_post',
                    date:     '2010-08-03',
                    title:    'Blog Post',
                    width:    400,
                    content:  '<b>Lorem Ipsum</b> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.',
                    image:    'images/evra.jpg',
                    readmore: 'http://www.manutd.com'
                }
            ];
            options       = {
                animation:   true,
                lightbox:    true,
                showYear:    false,
                allowDelete: false,
                columnMode:  'left'
            };
            break;
        case 4:
            timeline_data = [
                {
                    type:     'slider',
                    date:     '2011-12-16',
                    width:    400,
                    height:   150,
                    images:   ['images/group.jpg', 'images/old.jpg', 'images/win.jpg'],
                    speed:    5000
                },
                {
                    type:     'gallery',
                    date:     '2011-04-12',
                    title:    'Mini Gallery',
                    width:    400,
                    height:   100,
                    images:   ['images/rooney.jpg', 'images/tshirt.jpg', 'images/giggs.jpg', 'images/rio.jpg', 'images/paper.jpg']
                },
                {
                    type:     'blog_post',
                    date:     '2011-08-03',
                    title:    'Blog Post',
                    width:    400,
                    content:  '<b>Lorem Ipsum</b> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.',
                    image:    'images/rio.jpg',
                    readmore: 'http://www.manutd.com'
                },
                {
                    type:     'slider',
                    date:     '2010-12-16',
                    width:    400,
                    height:   200,
                    images:   ['images/ferguson.jpg', 'images/paper.jpg'],
                    speed:    5000
                },
                {
                    type:     'gallery',
                    date:     '2010-04-12',
                    title:    'Mini Gallery',
                    width:    400,
                    height:   150,
                    images:   ['images/stadium.jpg', 'images/rafel.jpg', 'images/logo.jpg', 'images/rvp.jpg']
                },
                {
                    type:     'blog_post',
                    date:     '2010-08-03',
                    title:    'Blog Post',
                    width:    400,
                    content:  '<b>Lorem Ipsum</b> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.',
                    image:    'images/evra.jpg',
                    readmore: 'http://www.manutd.com'
                }
            ];
            options       = {
                animation:   true,
                lightbox:    true,
                showYear:    false,
                allowDelete: false,
                columnMode:  'right'
            };
            break;
        case 5:
            timeline_data = [
                {
                    type:     'blog_post',
                    date:     '2012-09-03',
                    title:    'FA Cup',
                    width:    '95%',
                    content:  'The Reds go marching on in the FA Cup...',
                    image:    'images/facup.jpg'
                },
                {
                    type:     'blog_post',
                    date:     '2011-08-03',
                    title:    'Swansea',
                    width:    '95%',
                    content:  'Check out our exclusive video preview ahead of today\'s clash with Swansea <a href="http://bit.ly/Yz0bmZ" target="_blank">http://bit.ly/Yz0bmZ</a>',
                    image:    'images/rio.jpg'
                },
                {
                    type:     'blog_post',
                    date:     '2011-07-15',
                    title:    'Manchester United VS Liverpool',
                    width:    '95%',
                    content:  'The Reds complete the double over Liverpool this season...',
                    image:    'images/evra.jpg'
                },
                {
                    type:     'blog_post',
                    date:     '2011-06-29',
                    title:    'Michael Carrick',
                    width:    '95%',
                    content:  'Last chance to win Michael Carrick\'s signed shirt from the Liverpool game!! Click this link to enter <a href="http://bit.ly/W03U8k" target="_blank">http://bit.ly/W03U8k</a>',
                    image:    'images/carric.jpg'
                },
                {
                    type:     'blog_post',
                    date:     '2011-04-02',
                    title:    'Match',
                    width:    '95%',
                    content:  '9 Premier League wins out of 10 this season at Old Trafford. What is your match of the season so far at the Theatre of Dreams?',
                    image:    'images/wigan.jpg'
                },
                {
                    type:     'blog_post',
                    date:     '2010-02-13',
                    title:    'Old Traffordt',
                    width:    '95%',
                    content:  'Check out our exclusive video preview ahead of today\'s clash with Swansea <a href="http://bit.ly/Yz0bmZ" target="_blank">http://bit.ly/Yz0bmZ</a>',
                    image:    'images/home.jpg'
                }
            ];
            options       = {
                animation:   true,
                lightbox:    true,
                showYear:    true,
                allowDelete: false,
                columnMode:  'center'
            };
            break;
    }

    var timeline = new Timeline($('#timeline'), timeline_data);
    timeline.setOptions(options);
    timeline.display();
}






function twitterSearch() {
    $('#timeline').remove();
    var wrapper = $('<div>').attr('id', 'timeline').appendTo('#main');

    var hash_tag = $.trim($('#hashtag').val()).replace(/#/, '');
    if (hash_tag === '') {
        hash_tag = 'Envato';
    }

    $('#social_search').show();

    var timeline = new Timeline($('#timeline'));
    timeline.setOptions({
        animation:        true,
        lightbox:         true,
        showYear:         true,
        allowDelete:      false,
        columnMode:       'dual',
        defaultElementWidth: 300,
        twitterSearchKey: hash_tag,
        onSearchSuccess: function(data) {
            $('#social_search').hide();
        },
        onSearchError: function(data) {
            alert('No data found, please try another search key word.');
        }
    });
    timeline.display();
}

function facebookSearch(page_id) {
    $('#timeline').remove();
    var wrapper = $('<div>').attr('id', 'timeline').appendTo('#main');

    $('#social_search').show();

    var timeline = new Timeline($('#timeline'));
    timeline.setOptions({
        animation:           true,
        lightbox:            true,
        showYear:            true,
        allowDelete:         false,
        columnMode:          'dual',
        facebookPageId:      page_id ? page_id : $.trim($('#facebook_page_id').val()),
        facebookAccessToken: '593969810647709|iCNcvnyr1dT4oo566T9emHhq-go',
        onSearchSuccess: function(data) {
            $('#social_search').hide();
        },
        onSearchError: function(data) {
            alert('No data found, please try another page ID.');
        }
    });
    timeline.display();
}

function readRssFeed(rss_feed) {
    $('#timeline').remove();
    var wrapper = $('<div>').attr('id', 'timeline').appendTo('#main');

    if (typeof rss_feed === 'undefined') {
        rss_feed = $.trim($('#rss_feed').val());

        if (rss_feed === '') {
            rss_feed = 'http://www.digg.com/rss/index.xml';
        }

        $('#rss_feed').attr('placeholder', rss_feed);
    }

    $('#social_search').show();

    $.getJSON('https://ajax.googleapis.com/ajax/services/feed/load?v=1.0&num=10&q=' + rss_feed + '&callback=?', function(data) {
        $('#social_search').hide();

        var timeline_data = [];

        if (data && data.responseData && data.responseData.feed && data.responseData.feed.entries) {
            $(data.responseData.feed.entries).each(function(index, entry) {
                var date = entry.publishedDate.split(' ');

                var months = [];
                months['Jan'] = '01'; months['Feb'] = '02'; months['Mar'] = '03';
                months['Apr'] = '04'; months['May'] = '05'; months['Jun'] = '06';
                months['Jul'] = '07'; months['Aug'] = '08'; months['Sep'] = '09';
                months['Oct'] = '10'; months['Nov'] = '11'; months['Dec'] = '12';

                // Push Feed to Timeline Data
                timeline_data.push({
                    type:     'blog_post',
                    date:     date[3] + '-' + months[date[2]] + '-' + date[1],
                    title:    entry.title,
                    content:  entry.contentSnippet + '<div align="right"><a href="' + entry.link + '">Read More</a></div>',
                    width:    400
                });
            });

            // Build Timeline
            var timeline = new Timeline($('#timeline'), timeline_data);
            timeline.setOptions({
                animation:   true,
                lightbox:    true,
                showYear:    true,
                allowDelete: false,
                columnMode:  'dual',
                order:       'desc'
            });
            timeline.display();
        } else {
            alert('No results found, please try another RSS Feed');
        }
    });
}

var getTimelineData = function(years) {
    var data = [];

    var urls = [
        {
            url:         'http://3.s3.envato.com/files/61906715/admin7.png',
            link:        'http://themeforest.net/item/melonhtml5-admin7/5151011?ref=leli2000',
            description: 'Admin 7',
            inline:      'http://0.s3.envato.com/files/61405012/preview/inline_preview.__large_preview.png'
        },
        {
            url:         'http://0.s3.envato.com/files/54031436/royal_tab.png',
            link:        'http://codecanyon.net/item/melonhtml5-royal-tab/4116092?ref=leli2000',
            description: 'Royal Tab',
            inline:      'http://1.s3.envato.com/files/49472718/inline_preview.png'
        },
        {
            url:         'http://1.s3.envato.com/files/49500979/sliding_menu.png',
            link:        'http://codecanyon.net/item/melonhtml5-sliding-menu/3952436?ref=leli2000',
            description: 'Sliding Menu',
            inline:      'http://3.s3.envato.com/files/47296354/inline_preview.png'
        },
        {
            url:         'http://3.s3.envato.com/files/63387031/timeline.png',
            link:        'http://codecanyon.net/item/melonhtml5-timeline/3884755',
            description: 'Timeline',
            inline:      'http://3.s3.envato.com/files/46733366/timeline.png'
        },
        {
            url:         'http://2.s3.envato.com/files/50289678/royal_preloader.png',
            link:        'http://codecanyon.net/item/melonhtml5-royal-preloader/3746554?ref=leli2000',
            description: 'Preloader',
            inline:      'http://2.s3.envato.com/files/44935226/inline_preview.png'
        },
        {
            url:         'http://2.s3.envato.com/files/59939322/metro_gallery.png',
            link:        'http://codecanyon.net/item/melonhtml5-metro-gallery/3579821?ref=leli2000',
            description: 'Gallery',
            inline:      'http://3.s3.envato.com/files/42915391/inline_preview.png'
        },
        {
            url:         'http://1.s3.envato.com/files/43785447/emu_slider.png',
            link:        'http://codecanyon.net/item/melonhtml5-emu-slider/3123886?ref=leli2000',
            description: 'Emu Slider',
            inline:      'http://1.s3.envato.com/files/36000942/inline_preview.png'
        },
        {
            url:         'http://1.s3.envato.com/files/45390837/3d_cube_gallery.png',
            link:        'http://codecanyon.net/item/melonhtml5-3d-cube-gallery/2997743?ref=leli2000',
            description: 'Cube Gallery',
            inline:      'http://0.s3.envato.com/files/41707062/thumbnail.png'
        },
        {
            url:         'http://1.s3.envato.com/files/60352055/metro_ui.png',
            link:        'http://codecanyon.net/item/melonhtml5-metro-ui/2986068?ref=leli2000',
            description: 'Metro UI',
            inline:      'http://0.s3.envato.com/files/54025305/inline_preview.png'
        },
        {
            url:         'http://2.s3.envato.com/files/50913341/grid_slider.png',
            link:        'http://codecanyon.net/item/melonhtml5-grid-slider/4242803ref=leli2000',
            description: 'Grid Slider',
            inline:      'http://3.s3.envato.com/files/50707176/inline_preview.png'
        }
    ];


    $(years).each(function(index, year) {
        for (var i = 0; i <= 5; i++) {
            var item = urls[parseInt(Math.random() * urls.length)];

            data.push({
                type:     'blog_post',
                date:     year + '-08-' + Math.floor(Math.random() * 2 + 1) + Math.floor(Math.random() * 9),
                title:    item.description,
                width:    350,
                image:    item.inline,
                content:  '<a href="' + item.link + '">Lorem Ipsum</a> is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry\'s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.'
            });
        }
    });

    return data;
};

var init_loadmore = function() {
    var timeline = new Timeline($('#timeline'), getTimelineData([2013]));
    timeline.setOptions({
        animation:   true,
        lightbox:    true,
        showYear:    true,
        allowDelete: false,
        columnMode:  'dual',
        order:       'desc'
    });
    timeline.display();

    $(document).ready(function() {
        // menu click
        $(document).on('click', '#menu > div', function(e) {
            var year      = $(this).text();
            var scroll_to = year == 2013 ? '#timeline' : '#timeline_date_separator_' + year;

            $.scrollTo(scroll_to, 500);
        });

        // load more click
        var year = 2013;
        $('#loadmore').on('click', function(e) {
            var button = $(this);

            if (button.hasClass('loading')) {
                return;
            }

            year--;
            button.addClass('loading').text('Loading...');
            setTimeout(function() {
                var scroll_to = '#timeline_date_separator_' + year;

                button.removeClass('loading').text('Load More');
                $('<div>').text(year).appendTo($('#menu'));

                var new_data = getTimelineData([year]);
                timeline.appendData(new_data);
                $.scrollTo(scroll_to, 500);
            }, 1000);
        });
    });
};