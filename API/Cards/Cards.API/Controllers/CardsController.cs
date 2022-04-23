using Cards.API.Data;
using Cards.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cards.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly CardsDbContext cardsDbContext;

        public CardsController(CardsDbContext cardsDbContext)
        {
            this.cardsDbContext = cardsDbContext;
        }
        //Get All Cards
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await cardsDbContext.Cards.ToListAsync();
            return Ok(cards);
        }

        //get single card

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetCard")]
        public async Task<IActionResult> GetCard([FromRoute] Guid  id)
        {
            var card = await cardsDbContext.Cards.FirstOrDefaultAsync(p => p.Id == id);
            if(card!=null)
            {
                return Ok(card);
            }
            else
            {
                return NotFound("Card Not Found");
            }
            
        }

        //add a card

        [HttpPost]
        
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {
            card.Id = Guid.NewGuid();

            await cardsDbContext.Cards.AddAsync(card);
            await cardsDbContext.SaveChangesAsync();


            return CreatedAtAction(nameof(GetCard), new {id=card.Id},card);
        }


        //update a card

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCard([FromRoute] Guid id, [FromBody] Card card)
        {
            var returnedCard = await cardsDbContext.Cards.FirstOrDefaultAsync(p => p.Id == id);
            if (returnedCard != null)
            {
                returnedCard.CVC = card.CVC;
                returnedCard.CardHolderName = card.CardHolderName;
                returnedCard.CardNumber = card.CardNumber;
                returnedCard.ExpiryMonth = card.ExpiryMonth;
                returnedCard.ExpiryYear = card.ExpiryYear;

                await cardsDbContext.SaveChangesAsync();

                return Ok(returnedCard);

            }
            else
            {
                return NotFound("Card Not Found");
            }
        }


        //delete a card

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCard([FromRoute] Guid id)
        {
            var returnedCard = await cardsDbContext.Cards.FirstOrDefaultAsync(p => p.Id == id);
            if (returnedCard != null)
            {
                cardsDbContext.Cards.Remove(returnedCard);
                await cardsDbContext.SaveChangesAsync();

                return Ok(returnedCard);

            }
            else
            {
                return NotFound("Card Not Found");
            }
        }


    }
}
