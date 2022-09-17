(function ($) {
  const side = { client: "client", server: "server" }

  class ChatSession {
    isActive;
    chatUsers;
    messages = new Array();

    appendMessage = function (message) {
      messages.push(message);
    }

    startDateTime = function () {
      return messages.length != 0 ? messages[0] : null;
    };

    lastDateTime = function () {
      return messages.length != 0 ? messages[messages.length - 1] : null;
    };
  }

  class ChatUser {
    name;
    side;
    id;
  }

  class Message {
    text;
    sendDateTime;
    chatUser;
    send = function () {
      this.sendDateTime = new Date(Date.now());
    }
  }


  $.fn.chat = function (signalR, url) {
    $(document.body).append(`<style>
.chat-wrapper > .card {
    position:fixed;
    bottom:0;
    right:0;
    transition: height 150ms;
height:0;
width: 20rem;
z-index: 100;
}

.chat-wrapper.opened > .card {
height:initial;
    box-shadow: 0 0.5rem 1rem rgb(0 0 0 / 15%) !important;

}

.chat {
    overflow: hidden;
}
.chat .form-control {
    border-color: transparent;
    font-size: 0.75rem;
}
.chat .form-control:focus {
    border-color: transparent;
    box-shadow: inset 0px 0px 0px 1px transparent;
}
.chat .card-body {
    overflow-y: auto;
    overflow-x: hidden;
}
.divider:after,
.divider:before {
    content: "";
    flex: 1;
    height: 1px;
    background: #eee;
}
::-webkit-scrollbar-track {
    box-shadow: 0px 0px 3px #000 inset;
}
::-webkit-scrollbar-thumb {
    -webkit-border-radius: 5px;
    border-radius: 5px;
    background-color: rgb(13 110 253);
    box-shadow: 0px 1px 1px #005be1 inset;
}
::-webkit-scrollbar {
    width: 10px;
    height: 10px;
}

.chat-wrapper.opened #open_chat{
display:none;

}

#open_chat {
position:fixed;
bottom:5px;
right:5px;
-moz-transform: scale(-1, 1);
-o-transform: scale(-1, 1);
-webkit-transform: scale(-1, 1);
transform: scale(-1, 1);
}
</style>`);

    let $chatWrapper = $('<div class="chat-wrapper"></div>');
    let $openChat = $('<div id="open_chat"><i class="fas fa-3x fa-comment hovered" data-bs-original-title="Онлайн чат с менеджером" data-toggle="tooltip"></i></div>');

    $(document.body).append([$chatWrapper]);
    $chatWrapper.html('<div class="card"><div class="card-header d-flex justify-content-between align-items-center pe-0"> <h5 class="mb-0">Онлайн-консультант</h5><i class="fa fa-close p-3 hovered" aria-hidden="true"></i> </div> <div class="card-body" data-mdb-perfect-scrollbar="true" style="position: relative; height: 400px"> </div> <div class="card-footer text-muted d-flex justify-content-start align-items-center p-2"> <input type="text" class="form-control form-control-md" id="outgoing_message" placeholder="Сообщение"> <a class="ms-2 text-muted" href="#!"><i class="fas fa-paperclip"></i></a> <a class="ms-3" href="#!"><i class="fas fa-paper-plane"></i></a> </div> </div>');
    $chatWrapper.append($openChat);

    let $filePickerIcon = $chatWrapper.find('.fas.fa-paperclip');
    let $sendButton = $chatWrapper.find('.fas.fa-paper-plane');
    let $outgoingMessageWrapper = $chatWrapper.find('#outgoing_message');

    let $chat = $chatWrapper.find('.card');
    let $closeButton = $chatWrapper.find('.fa.fa-close');


    $closeButton.on('click', (evt) => {
      $chatWrapper.removeClass('opened');
    });

    $filePickerIcon.on('click', function () {
      let $this = $(this);
      let $filePicker = $('<input type="file" style="display:none" name="uploadFile"/>');
      $filePicker.on('change', function (evt) {
        let files = Array.from(evt.target.files);
        files.forEach(file => {
          console.log(file);
        });
      });
      $filePicker.insertAfter($this);
      $filePicker.click();
    });

    $openChat.on('click', (evt) => {
      $chatWrapper.toggleClass('opened');
    });

    $sendButton.on('click', sendMessage);
    $outgoingMessageWrapper.on('keyup', (evt) => { if (evt.key == 'Enter') sendMessage(); });

    const chatConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${url}`)
      .build();

    function sendMessage() {
      if ($outgoingMessageWrapper.val() == '') return;
      chatConnection.invoke('MessageFromClient', $outgoingMessageWrapper.val());
      $outgoingMessageWrapper.val('');
    }

    //chatConnection.on("MessageFromClient", function (obj) {
    //  console.log(obj);
    //});

    chatConnection.on("MessageFromServer", function (obj) {
      console.log(obj);
    });

    chatConnection.start();
  };
})(jQuery);

//$('.chat').chat(signalR, '/chat');
