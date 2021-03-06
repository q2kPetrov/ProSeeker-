﻿namespace ProSeeker.Data.Models.PrivateChat
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using ProSeeker.Data.Common.Models;

    public class ChatMessage : BaseDeletableModel<string>
    {
        public ChatMessage()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [MaxLength(2500)]
        public string Content { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public bool IsSeenBySender { get; set; }

        public bool IsSeenByReceiver { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        [Required]
        public string ConversationId { get; set; }

        public Conversation Conversation { get; set; }
    }
}
