; Helper function - do not modify
(define (mydisplay value)
	(display value)
	(newline)
	#t
)



; You are allowed to introduce additional helper functions; as you see fit.


; Returns the union of two sets. The inputs are flat lists
; of atoms. The result is a flat list containing all the elements
; that appear in either one list or the other. No duplicates 
; are present in the result. Order is not important.
; (union '(a b c) '(1 2 a b c)) -> (a b c 1 2)
; (union '(a b c) '(1 2 a b c 0)) -> (a b c 1 2 0)
(define (union lst1 lst2)
        (cond ((NULL? lst2) lst1)
	      ((member (car lst2) lst1) 
	        (union lst1 (cdr lst2)))
	      (else (union (cons (car lst2) lst1) 
		    (cdr lst2)))
	) 
)

(mydisplay "UNION --------------------------------------")
(mydisplay (union '(a b c) '(1 2 a b c)))
(mydisplay (union '(a b c) '(1 2 a b c 0)))
(mydisplay (union '(d e f) '(1 2 a b c 0)))



; Returns the roots of the quadratic formula, given
; the quadratic equation ax^2+bx+c=0. Return only the 
; real roots. The list will contain 0, 1, or two unique 
; real roots.
; Hint: use the "real?" predicate
(define (quadratic a b c)
  (define descri (- (* b b) (* 4 a c )))      
       (cond
	 ((= a 0)
	   '("attempted to divide by 0")
	  )
	 ((< descri 0) 
	        '()) 
	 ((= descri 0) 
	         (LIST (/ (- (- 0 b) (SQRT descri)) (* 2 a)))
	   )
         (else (> descri 0)      	   
	       (LIST (/ (- (- 0 b) (SQRT descri)) (* 2 a))
	             (/ (+ (- 0 b) (SQRT descri)) (* 2 a)))     
	  ) 
	)
)

(mydisplay "QUADRATIC --------------------------------------")
(mydisplay (quadratic 1 0 0))
(mydisplay (quadratic 0 1 0))
(mydisplay (quadratic 3 4 2))
(mydisplay (quadratic 5 6 1))
(mydisplay (quadratic 5 2 1))
(mydisplay (quadratic 1 2 1))



; Returns a list with the original order reversed.
; The function must use tail recursion (introduce helper function).
; (reverseTail '(a b c)) -> (c b a)
; (reverseTail '(a (a a) b) -> (b (a a) a)
; (reverseTail '(0)) -> (0)
;


(define (reverse_helper lst reverse_partial)
  (if (NULL? lst)
     reverse_partial
    (reverse_helper (cdr lst) (cons (car lst) reverse_partial))
  )
)    

(define (reverseTail lst)
	(if (NULL? lst)
	    '()
	    (reverse_helper lst '())
	  )
)

(mydisplay "REVERSETAIL --------------------------------------")
(mydisplay (reverseTail '(a b c)))
(mydisplay (reverseTail '(a (a a) b)))
(mydisplay (reverseTail '(0)))



; compose takes two functions F1 and F2, and returns a new function 
; that is the composition F1oF2. The two inputs are lambda functions.
; Assume that both F1 and F2 take only one parameter.
(define (compose F1 F2)

        (define f (eval F1 (interaction-environment))) ; stub for scheme 48
        (define g (eval F2 (interaction-environment)))
	
	(LAMBDA (x) (f (g x)))
)

(define square '(lambda (x) (* x  x)))
(define cube '(lambda (x) (* x  x x)))
(define clamp '(lambda (x) (if (< x 0) 0 x)))

(define cubeOfClamp (compose cube clamp))
(define sqrOfCube (compose square cube))
(define clampOfCube (compose clamp cube))

(mydisplay "COMPOSE --------------------------------------")
(mydisplay (cubeOfClamp -2))
(mydisplay (cubeOfClamp 2))
(mydisplay (sqrOfCube -2))
(mydisplay (sqrOfCube 2))
(mydisplay (clampOfCube -2))
(mydisplay (clampOfCube 2))



; sales.scm contains all the company's sales.
; You should not modify the contents of the sales.scm file. 
; Your code should work for other instances of this file,
; for instance salesBig.scm
(load "sales.scm")



; Returns the order information, given a specific order number.
; Returns the empty list, if there is no record of an order with the
; given orderNo.
; (getOrder SALES 3) -> (3 ("10/13/2010" "10/20/2010") (261.54 0.04 -213.25 38.94) ("Regular Air" "Nunavut") "Eldon Base for stackable storage shelf, platinum")
; (getOrder SALES 1) -> ()
(define (getOrder sales orderNo)
	(if (NULL? sales)
	  '()
	  (if (=(caar sales) orderNo)
	      (cons (car sales) (getOrder (cdr sales) orderNo))
	      (getOrder (cdr sales) orderNo)
	    )
	  )
)

(mydisplay "GETORDER --------------------------------------")
(mydisplay (getOrder SALES 1))
(mydisplay (getOrder SALES 3))
(mydisplay (getOrder SALES 293))



; Returns the profit information out of a given record for a sale.
; (getProfit '(3 ("10/13/2010" "10/20/2010") (261.54 0.04 -213.25 38.94) ("Regular Air" "Nunavut") "Eldon Base for stackable storage shelf, platinum")) -> -213.25
(define (getProfit sale)
        (CADDR (CADDR sale))
)

(mydisplay "GETPROFIT --------------------------------------")
(mydisplay (getProfit '(3 ("10/13/2010" "10/20/2010") (261.54 0.04 -213.25 38.94) ("Regular Air" "Nunavut") "Eldon Base for stackable storage shelf, platinum")))
(mydisplay (getProfit '(293 ("10/1/2012" "10/2/2012") (10123.02 0.07 457.81 208.16) ("Delivery Truck" "Northwest Territories") "1.7 Cubic Foot Compact Cube Office Refrigerators")))



; Returns the total profits for all sales. Returned
; orders are not included in this total.
(define (totalProfits sales returns)
	(COND ((NULL? sales) 0) 
              ((member (caar sales) returns)
			(totalProfits (cdr sales) returns))
              (else 
		
	       (+ (getProfit (car sales)) (totalProfits (cdr sales) returns)))     
		
	)
)

(mydisplay "TOTALPROFITS --------------------------------------")
(mydisplay (totalProfits SALES RETURNS))



; Returns the province information out of a given record for a sale.
; (getProv '(3 ("10/13/2010" "10/20/2010") (261.54 0.04 -213.25 38.94) ("Regular Air" "Nunavut") "Eldon Base for stackable storage shelf, platinum")) -> Nunavut
(define (getProv sale)
        (cadr (cadddr sale))
)

(mydisplay "GETPROV --------------------------------------")
(mydisplay (getProv '(3 ("10/13/2010" "10/20/2010") (261.54 0.04 -213.25 38.94) ("Regular Air" "Nunavut") "Eldon Base for stackable storage shelf, platinum")))
(mydisplay (getProv '(293 ("10/1/2012" "10/2/2012") (10123.02 0.07 457.81 208.16) ("Delivery Truck" "Northwest Territories") "1.7 Cubic Foot Compact Cube Office Refrigerators")))

(define (getProvList sales)
        (if (NULL? sales)
	    '()
	    (cons (getProv (car sales)) (getProvList (cdr sales)))
	 )
 )

(define (remove_duplicate sales)
       (cond ((NULL? sales)
               '()
	      )

	     ((member (car sales) (cdr sales))
	      (remove_duplicate (cdr sales))
	      )

             (else 
	      (cons (car sales) (remove_duplicate (cdr sales)))
	      )
	 )
)

; Returns the set of  provinces that the company sold to.
(define (getProvinces sales)
        (if (NULL? sales)
	    '()
            (remove_duplicate (getProvList sales))
	  )
)

(mydisplay "GETPROVINCES --------------------------------------")
(mydisplay (getProvinces SALES))

; helper method for calculating the profit sums of a province
(define (profitSum sales prov returns)
        (cond 
	      ((NULL? sales) 0) 
	      ((member (caar sales) returns) (profitSum (cdr sales) prov returns))
              ((EQUAL? (getProv (car sales)) prov) (+ (getProfit (car sales)) (profitSum (cdr sales) prov returns))) 
              (else  (profitSum (cdr sales) prov returns))
	)
 )

; helper method for generating (province, profit) list 
(define (proProfitList provinces returns sales)
        (if (NULL? provinces)
	  '()
          (cons (list (car provinces) (profitSum sales (car provinces) returns)) 
		(proProfitList (cdr provinces) returns sales))
	) 
)

; Returns the provinces with their profits from that
; province. These are total profits for each province.
(define (provincialProfit sales returns)
        (if (NULL? sales)
	   '()
	   (proProfitList (getProvinces sales) returns sales) 
	 )
)

(mydisplay "PROVINCIALPROFIT --------------------------------------")
(mydisplay (provincialProfit SALES RETURNS))

,exit
