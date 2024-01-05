using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MatchingGame
{
    public partial class MainPage : ContentPage
    {
        private List<string> symbols;
        private List<Button> cards;
        private Button firstCard;
        private Button secondCard;
        private bool isBusy;

        public MainPage()
        {
            InitializeComponent();
            symbols = new List<string> { "A", "A", "B", "B", "C", "C", "D", "D" };
            cards = new List<Button> { card1, card2, card3, card4, card6, card7, card8, card9 };
            ShuffleCards();
        }

        private void ShuffleCards()
        {
            Random random = new Random();
            symbols = symbols.OrderBy(s => random.Next()).ToList();
        }

        private async void OnCardClicked(object sender, EventArgs e)
        {
            if (isBusy)
                return;

            var clickedCard = (Button)sender;

            if (clickedCard == null || clickedCard == firstCard || clickedCard.Text != "")
                return;

            if (firstCard == null)
            {
                firstCard = clickedCard;
                await FlipCard(firstCard, symbols[cards.IndexOf(firstCard)]);
            }
            else if (secondCard == null)
            {
                secondCard = clickedCard;
                await FlipCard(secondCard, symbols[cards.IndexOf(secondCard)]);

                if (firstCard.Text == secondCard.Text)
                {
                    firstCard = null;
                    secondCard = null;

                    if (symbols.All(symbol => cards.Any(card => card.Text == symbol)))
                    {
                        // All cards matched, display a message and reset the game
                        await DisplayAlert("Game Over", "Congratulations! You matched all the cards.", "OK");
                        ResetGame();
                    }
                }
                else
                {
                    isBusy = true;
                    await System.Threading.Tasks.Task.Delay(1000);
                    await FlipCard(firstCard, "");
                    await FlipCard(secondCard, "");
                    firstCard = null;
                    secondCard = null;
                    isBusy = false;
                }
            }
        }

        private async System.Threading.Tasks.Task FlipCard(Button card, string symbol)
        {
            await card.RotateTo(90, 250, Easing.Linear);
            card.Text = symbol;
            await card.RotateTo(180, 250, Easing.Linear);
        }

        private void ResetGame()
        {
            // Reset the game by shuffling symbols and hiding the text on all buttons
            ShuffleCards();
            foreach (var card in cards)
            {
                card.Text = "";
            }
        }

        private void OnReset(object sender, EventArgs e)
        {
            // Handle the Reset button click event
            ResetGame();
        }
    }
}
