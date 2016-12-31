require('dotenv').config({path: 'settings.env'});
var BotKit = require('BotKit');
var ngrok = require('ngrok');
var axios = require('axios');

axios.defaults.headers.common['Authorization'] = `OAuth ${process.env.PAGE_TOKEN}`;

var controller = BotKit.facebookbot({
  debug: true,
  log: true,
  access_token: process.env.PAGE_TOKEN,
  verify_token: process.env.VERIFY_TOKEN,
  app_secret: process.env.APP_SECRET,
  validate_requests: true,
  require_delivery: true
});

var bot = controller.spawn({});

controller.setupWebserver(3000, function(err, webserver) {
  controller.createWebhookEndpoints(webserver, bot, function() {
    console.log("Bot intialised");
    
    if(process.env.NGROK === 'true') {
      ngrok.connect({
        addr: 3000,
        authtoken: process.env.NGROK_TOKEN
      }, function(err, url) {
        if(err) {
          console.log(err);
          process.exit(0);
        }

        console.log(`Bot can be accessed from ${url}/facebook/receive`);
      });
    }
  });
});

controller.api.thread_settings.greeting('Hi I\'m Skyla, A bot part of the Christmas Coding 2016');
controller.api.thread_settings.get_started('get_started');
controller.api.thread_settings.menu([
    {
        "type":"postback",
        "title":"Get Started",
        "payload":"get_started"
    },
    {
        "type":"postback",
        "title":"Recommend me a film",
        "payload":"recommend"
    }
]);

// Setting Whitelist domain
axios.post(`${process.env.FACEBOOK_API}me/thread_settings?access_token=${process.env.PAGE_TOKEN}`, {
  "setting_type" : "domain_whitelisting",
  "whitelisted_domains" : [process.env.API_URL],
  "domain_action_type": "add"
}).then(function(response) {
  console.info('debug:', 'Successfully configured thread settings - Added Domain');
}).catch(function(err) {
  console.error(err)
});

controller.hears(['^recommend'], 'message_received,facebook_postback', function(bot, message) {
  var doneCallback = function() {
    bot.reply(message, "Getting recomendation now, this can take around 3 minutes");

    axios.get(`${process.env.API_URL}api/recomendation/${message.user}`)
      .then(function({data}) {
        bot.reply(message, {
          'attachment': {
            'type': 'template',
            'payload': {
              'template_type': 'generic',
              'elements': [
                {
                  'title': data.Title,
                  'image_url': `${process.env.SKY_IMAGE_URL}${data.Uuid}/cover`,
                  'subtitle' : data.Synopsis
                }
              ]
            }
          }
        });
      })
      .catch(function(err) {
        console.error(err);
      });
  };

  checkAuth(bot, message, doneCallback);
});

var checkAuth = function(bot, message, doneCallback) {
  axios.get(`${process.env.API_URL}api/fuser/GetAuth/${message.user}`)
    .then(function({data}) {
      if(data.isAuthed == false) {
        bot.reply(message, {
          attachment: {
            'type': 'template',
            'payload': {
              'template_type': 'generic',
              'elements': [
                {
                  'title':'We just need to reauthorise you...',
                  'buttons':[
                    {
                      'type': 'web_url',
                      'url': `${process.env.API_URL}Facebook/ReAuth`,
                      "messenger_extensions": true,
                      "webview_height_ratio": "compact",
                      'title':'Reauth Me!',
                    }
                  ]
                }
              ]
            }
          }
        });

        bot.reply(message, 'Once the window has closed please try that action again');
      } else if(data.isAuthed == true) {
        doneCallback();
      }
    }).catch(function(err) {
      console.error(error);
    });
};

controller.hears(["^get_started"], 'message_received,facebook_postback', function(bot, message) {
  bot.reply(message, {
    attachment: {
      'type': 'template',
      'payload': {
        'template_type': 'generic',
        'elements': [
          {
            'title':'Link to Facebook',
            'buttons':[
              {
                'type': 'account_link',
                'url': `${process.env.API_URL}Facebook/Link`
              }
            ]
          }
        ]
      }
    }
  });
});