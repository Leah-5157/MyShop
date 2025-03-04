const snowFall = (() => {
    //----------------------------------
    // Constants
    //----------------------------------

    // Quantity of snowflakes generated in average each second on each 100px section
    const FLAKE_FREQUENCY = 7; //7
    // Speed of the slowest snowflake (px/sec) before wind noise
    const FLAKE_MIN_SPEED = 30;
    // Speed of the fastest snowflake (px/sec) before wind noise
    const FLAKE_MAX_SPEED = 180;
    // Noise applicated to the flakes sizes
    const FLAKE_SIZE_NOISE = 0.5;
    // Radius of the furthest snowflake in px before size noise
    const FLAKE_MIN_SIZE = 0.4;
    // Radius of the nearest snowflake in px before size noise
    const FLAKE_MAX_SIZE = 1.6;
    // Friction coefficient applicated by the air to the flakes [0; 1]
    const FLAKE_FRICTION = 0.035;
    // Maximum force applicated to the flakes to stray from x axis
    const FLAKE_NOISE_X = 0.07;
    // Maximum force applicated to the flakes to stray from y axis
    const FLAKE_NOISE_Y = 0.02;
    // Radius of influence of the mouse move (ratio of the width or height of the screen)
    const FLAKE_CAST_RADIUS_RATIO = 0.4;
    // Force applicated to the flakes on mouse mouve
    const FLAKE_CAST_FORCE = 0.05;
    // Minimum depth to trigger a cast on mouse move
    const FLAKE_CAST_DEPTH_TRIGGER = 0.4;
    // Delay in ms between two consecutive snow casts
    const FLAKE_CAST_THROTTLE = 50;
    // Useful shortcut
    const PI = Math.PI;
    // Estimated framerate
    const FPS = 60;

    //----------------------------------
    // Objects
    //----------------------------------

    class Point {
        static distance(a, b) {
            return Math.sqrt(Math.pow(b.y - a.y, 2) + Math.pow(b.x - a.x, 2));
        }

        constructor(x = 0, y = 0) {
            this.x = x;
            this.y = y;
        }

        translate(translateVect) {
            this.x += translateVect.x;
            this.y += translateVect.y;
        }
    }

    class Vector {
        static add(vectors) {
            let result = new Vector(0, 0);
            vectors.forEach((vector) => {
                result.x += vector.x;
                result.y += vector.y;
            });
            return result;
        }

        constructor(x = 0, y = 0) {
            this.x = x;
            this.y = y;
        }

        get length() {
            return Math.sqrt(Math.pow(this.x, 2) + Math.pow(this.y, 2));
        }
    }

    class Particle {
        static deduceTargetSpeed(mass, friction) {
            return new Vector(0, mass / FLAKE_FRICTION);
        }

        static deduceMass(targetSpeed, friction) {
            return targetSpeed.y * FLAKE_FRICTION;
        }

        constructor(
            position = { x: 0, y: 0 },
            { mass = 0, friction = 0, initialSpeed = { x: 0, y: 0 } }
        ) {
            this.position = new Point(position.x, position.y);
            this.mass = mass;
            this.friction = friction;
            this.speed = new Vector(initialSpeed.x, initialSpeed.y);
            this.forces = new Map();
            this.applyPhysics();
        }

        setForce(forceName, forceValue = { x: 0, y: 0 }) {
            this.forces.set(forceName, new Vector(forceValue.x, forceValue.y));
        }

        applyGravity() {
            this.setForce("weight", { x: 0, y: this.mass });
        }

        applyFriction() {
            this.setForce("friction", {
                x: -this.speed.x * this.friction,
                y: -this.speed.y * this.friction
            });
        }

        applyPhysics() {
            if (this.mass) this.applyGravity();
            if (this.friction) this.applyFriction();
            const acceleration = Vector.add(this.forces);
            this.forces.clear();
            this.speed = Vector.add([this.speed, acceleration]);
            this.position.translate(this.speed);
        }
    }

    class Flake extends Particle {
        constructor(position = { x: 0, y: 0 }) {
            const depth = random(0, 100) / 100;
            const initialSpeed = {
                x: 0,
                y: (FLAKE_MIN_SPEED + depth * (FLAKE_MAX_SPEED - FLAKE_MIN_SPEED)) / FPS
            };
            const mass = Particle.deduceMass(initialSpeed, FLAKE_FRICTION);

            super(position, {
                mass: mass,
                friction: FLAKE_FRICTION,
                initialSpeed: { x: initialSpeed.x, y: initialSpeed.y }
            });

            this.depth = depth;
            this.size = FLAKE_MIN_SIZE + depth * (FLAKE_MAX_SIZE - FLAKE_MIN_SIZE);
            this.size =
                this.size * (1 + FLAKE_SIZE_NOISE * (random(-100, 100) / 100));
            this.noiseSpeed = new Vector(0, 0);
        }

        evolve() {
            if (this.depth < FLAKE_CAST_DEPTH_TRIGGER && this.forces.get("cast"))
                this.forces.delete("cast");
            this.applyPhysics();
            this.addNoise();
        }

        addNoise() {
            this.noiseForce = new Vector(
                (random(-100, 100) / 100) * FLAKE_NOISE_X * this.depth,
                (random(-100, 100) / 100) * FLAKE_NOISE_Y * this.depth
            );
            this.noiseSpeed = Vector.add([this.noiseSpeed, this.noiseForce]);
            this.position.translate(this.noiseSpeed);
        }

        draw(ctx) {
            ctx.beginPath();
            ctx.arc(this.position.x, this.position.y, this.size, 0, 2 * PI);
            ctx.fillStyle = "white";
            ctx.fill();
        }
    }

    class ForceField {
        constructor(forceName, initialWidth, initialHeight) {
            this.forceName = forceName;
            this.reset(initialWidth, initialHeight);
        }

        reset(l, h) {
            const targetWidth = l ? l : this.width;
            const targetHeight = h ? h : this.height;
            this.forces = Array(targetWidth)
                .fill(null)
                .map(() => Array(targetHeight).fill(null));
            this.isEmpty = true;
            this.width = targetWidth;
            this.height = targetHeight;
        }

        getForceAt(n, m) {
            if (this.isEmpty) return;
            let force;
            try {
                force = this.forces[n][m];
            } catch {
                return;
            }
            if (!force) return;
            if (!("x" in force && "y" in force)) {
                console.warn(
                    `${this.forceName} : The value found at ${n} ${m} is not a force`
                );
                return;
            }
            return { x: force.x, y: force.y };
        }

        setForceAt(n, m, value) {
            let force;
            try {
                force = Object.assign({}, { x: value.x, y: value.y });
            } catch {
                console.warn(
                    `${this.forceName} : The value provoded to be set at ${n} ${m} is not a force`
                );
            }
            try {
                this.forces[n][m] = Object.assign({}, force);
            } catch {
                console.warn(`${this.forceName} : Failed to set ${n} ${m} force`);
                return;
            }
            this.isEmpty = false;
        }

        applyField(particles) {
            if (this.isEmpty) return;
            particles.forEach((particle, index) => {
                if (!(particle instanceof Particle)) {
                    console.warn(
                        `The ${index}th Object provided to the ${this.forceName} force is not a particle`
                    );
                    return;
                }
                const x = Math.round(particle.position.x);
                const y = Math.round(particle.position.y);
                const force = this.getForceAt(x, y);

                if (force) particle.setForce(this.forceName, force);
            });
            this.reset();
        }
    }

    class FlakeCaster extends ForceField {
        calculateField(x, y, force, castRadius) {
            const boundary = Math.ceil(castRadius / 2);
            const iMin = x - boundary > 0 ? x - boundary : 0;
            const iMax = x + boundary < this.width ? x + boundary : this.width;
            const jMin = y - boundary > 0 ? y - boundary : 0;
            const jMax = y + boundary < this.height ? y + boundary : this.height;
            for (let i = iMin; i < iMax; i++) {
                for (let j = jMin; j < jMax; j++) {
                    const distanceToEpicenter = Point.distance(
                        { x: x, y: y },
                        { x: i, y: j }
                    );
                    let intensity =
                        FLAKE_CAST_FORCE * (1 - distanceToEpicenter / boundary);
                    intensity = intensity < 0 ? 0 : intensity;
                    this.setForceAt(i, j, {
                        x: force.x * intensity,
                        y: force.y * intensity
                    });
                }
            }
        }
    }

    //----------------------------------
    // Utils
    //----------------------------------

    function random(min, max) {
        const range = Math.abs(max) + Math.abs(min);
        return Math.round(Math.random() * range + min);
    }

    function chance(probability) {
        return Math.random() < probability ? true : false;
    }

    //----------------------------------
    // Main instructions
    //----------------------------------

    this.init = function () {
        const canvas = document.getElementById("snowfall");
        const ctx = canvas.getContext("2d");
        let { width, height } = canvas;

        const flakes = [];
        let flakeRequestPerFrame;
        let castRadius;

        const castField = new FlakeCaster("cast", width, height);
        const mouse = {
            currentPosition: { x: 0, y: 0 },
            prevPosition: { x: 0, y: 0 },
            move: function (x, y) {
                this.prevPosition = Object.assign({}, this.currentPosition);
                this.currentPosition.x = x;
                this.currentPosition.y = y;
            },
            get vitesse() {
                return {
                    x: this.currentPosition.x - this.prevPosition.x,
                    y: this.currentPosition.y - this.prevPosition.y
                };
            }
        };

        function resize() {
            const { innerWidth, innerHeight } = window;
            canvas.width = innerWidth;
            canvas.height = innerHeight;
            width = innerWidth;
            height = innerHeight;
            castRadius = FLAKE_CAST_RADIUS_RATIO * Math.min(width, height);
            flakeRequestPerFrame = ((width / 100) * FLAKE_FREQUENCY) / FPS;
            castField.reset(width, height);
        }

        function castFlakes(e) {
            let x, y;
            if (e.type === "mousemove") {
                x = e.clientX;
                y = e.clientY;
            }
            if (e.type === "touchmove") {
                x = e.touches[0].clientX;
                y = e.touches[0].clientY;
            }

            mouse.move(x, y);
            if (mouse.vitesse.x && mouse.vitesse.y) {
                const force = Object.assign(
                    {},
                    { x: mouse.vitesse.x, y: mouse.vitesse.y }
                );
                castField.calculateField(x, y, force, castRadius);
            }
        }

        function draw() {
            ctx.clearRect(0, 0, width, height);

            castField.applyField(flakes);

            let nbFlakeToCreate = Math.floor(flakeRequestPerFrame);
            if (chance(flakeRequestPerFrame % 1)) nbFlakeToCreate++;
            for (let i = 0; i < nbFlakeToCreate; i++) {
                flakes.push(new Flake({ x: random(0, width), y: 0 }));
            }

            flakes.forEach((flake, index, flakes) => {
                if (
                    !(
                        _.inRange(flake.position.y, height) &&
                        _.inRange(flake.position.x, width)
                    )
                ) {
                    flakes.splice(index, 1);
                }
                flake.evolve();
                flake.draw(ctx);
            });

            window.requestAnimationFrame(draw);
        }

        resize();
        window.onresize = resize;
        window.onmousemove = _.throttle(castFlakes, FLAKE_CAST_THROTTLE);
        window.addEventListener(
            "touchmove",
            _.throttle(castFlakes, FLAKE_CAST_THROTTLE)
        );
        draw();
    };

    return this;
})();

document.addEventListener("DOMContentLoaded", snowFall.init);