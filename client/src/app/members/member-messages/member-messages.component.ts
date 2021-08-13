import {
  ChangeDetectionStrategy,
  Component,
  Input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from 'src/app/models/message';
import { MessageService } from 'src/app/services/message.service';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm: NgForm;
  @Input() username: string;
  @Input() messages: Message[];
  messageContent: string;
  loading = false;
  constructor(public messageService: MessageService) {}

  ngOnInit(): void {}

  sendMessage() {
    //Since we are using the Signal R service for sending the messages
    // this is not using http request and so ideally interceptor will not work
    // so we are using the spinner now.
    this.loading =true;
    this.messageService
      .sendMessage(this.username, this.messageContent)
      .then(() => {
        this.messageForm.reset();
      }).finally(()=> this.loading = false);
  }
}
